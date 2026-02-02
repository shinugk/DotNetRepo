using System.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using InterviewTracker.Utils.Validation;
using System.Text.Json.Serialization;


namespace InterviewTracker.Models
{
    public class Employer
    {
        [Key]
        public int id { get; set; }

        [Required]
        [MaxLength(100)]
        public string companyName { get; set; }

        [Required]
        [ValueOneOf(AllowedValues = $"{CompanyType.ServiceBased},{CompanyType.ProductBased},{CompanyType.Consulting}")]
        public string type { get; set; }

        [Url]
        public string? websiteLink { get; set; }  //That ? is part of C# Nullable Reference Types (introduced in C# 8.0).

        //LongBlob for file storage
        public byte[]? offerLetter { get; set; }    // file path or byte[] (file path to fetch from cloud or byte[] to store in own db)
       
        public decimal? ctcOffered { get; set; }

        [ValueOneOf(AllowedValues = $"{InterviewStatus.Selected},{InterviewStatus.NotSelected},{InterviewStatus.InProgress}")]
        public string? interviewStatus { get; set; }

        public string? location { get; set; }

        //Notes (long description) if you don't specify MaxLength() then it will be LONGTEXT
        public string? notes { get; set; }  //VARCHAR(255) is too small for long notes, interview feedback. TEXT & LONGTEXT can store very long strings

        //Date of joining (if selected)
        [Column(TypeName = "DATE")]     // Force DB column to DATE -> If you only want to store date
        public DateTime? dateOfJoining { get; set; }

        //Offered role
        [MaxLength(100)]
        public string? offeredRole { get; set; }

        //Interview round info
        [MaxLength(50)]
        public string? interviewLevel { get; set; }      // Examples: "HR", "Technical Round 1", "Technical Round 2", "Managerial"



        // 🔗 Foreign Key to User
        [ForeignKey("user")]                     //The string "user" must match with the name of a navigation property in the same entity class.
        public int userId { get; set; }

        // Navigation property (parent reference)
        [JsonIgnore]                                       //JsonIgnore for preventing back reference & circular reference (User → Employers → User → Employers → User...) Preventing Infinity loop. //otherwise use DTO's which handles serialization. not a db change. not required to add migration
        public User user { get; set; }   

        // 🔗 Navigation: One Employer → One HR
        public HRDetail hrDetail { get; set; }
    }

    public static class CompanyType
    {
        public const string ServiceBased = "Service Based";
        public const string ProductBased = "Product Based";
        public const string Consulting = "Consulting";
    }

    public static class InterviewStatus
    {
        public const string Selected = "Selected";
        public const string NotSelected = "Not Selected";
        public const string InProgress = "In-Progress";
    }


    /*
    🔑 Key Rule in EF Core
    -------------------------------------------------------------------------------
    Foreign key is always stored on the DEPENDENT (child) entity, not the principal (parent).

    In your case:
    User → Parent (Principal)
    Employer → Child (Dependent)

    So the foreign key must be in Employer, not in User.



    ❌ If you add EmployerId or UserId in User, then:
    ----------------------------------------------------------
    One user can point to only one employer
    OR you would need multiple columns → impossible
    Violates normalization



    🔥 How EF Core Understands This Automatically
    EF Core sees:
    User has ICollection<Employer>
    Employer has UserId + User

    So EF Core infers: User.Id  ←── Employer.UserId
    No extra configuration needed.


    🧪 Quick Interview Question (Very Important)
    Q: Where does foreign key exist in One-to-Many?
    ✔ Answer: On the Many (dependent) side




    ✅ BEST PRACTICE:  Configure in Fluent API 
    -------------------------------------------------------
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employer>()
            .HasOne(e => e.HRDetail)
            .WithOne(h => h.Employer)
            .HasForeignKey<HRDetail>(h => h.EmployerId);
    }
    ✔ This guarantees 1 Employer → 1 HRDetail
    ✔ Prevents multiple HR records for one employer


    ❓ Interview Question Tip
    Q: Where is FK stored in One-to-One?
    ✔ Answer: On the dependent entity (HRDetail)

    🔑 Rule to Remember (Interview Gold ⭐)
    Foreign Key always lives in the DEPENDENT entity.
    In Many-to-One, the Many side is the dependent side.




    public string? websiteLink { get; set; }  //That ? is part of C# Nullable Reference Types (introduced in C# 8.0).
    websiteLink is allowed to be null
    before this caused many NullReferenceException bugs at runtime.
    🏆 Interview Answer:
    The ? indicates a nullable reference type, meaning the variable is allowed to be null and the compiler will enforce null-safety.





    Do we even need [ForeignKey] attribute?  
    -----------------------------------------
    Most of the time: NO                            (Needed only when there are multiple foreign keys to same table)
    EF Core follows convention-based mapping
    Recommended (clean way):
    public class Employer
    {
        public int UserId { get; set; } 

        public User User { get; set; }
    }



    Other types to store long string
    | Type           | Max size           |
    |-------------- | ------------------ |
    | `VARCHAR(255)` | 255 characters     |
    | `TEXT`         | ~65,535 characters |
    | `MEDIUMTEXT`   | ~16 MB             |
    | `LONGTEXT`     | ~4 GB              |    //By Default all other string properties are LONGTEXT if you don't mention MaxLength(100)



    DATETIME:
    ---------------------------------------------------------
     How DateTime is stored (IMPORTANT)
    ⚠️ DateTime does NOT store timezone info by default
    It only stores:   YYYY-MM-DD HH:mm:ss
    So: DateTime.Now    stores server local time, not user time.

    ✅ Always store UTC in database
    public DateTime? dateOfJoining { get; set; }
    When saving:
    dateOfJoining = DateTime.UtcNow;

    How API sends & receives DateTime
    JSON request:
    {
      "dateOfJoining": "2026-02-01T00:00:00Z"
    }
    C# receives:  DateTime dateOfJoining;
    Z → UTC

    👉 Force DB column to DATE -> If you only want to store date
    [Column(TypeName = "DATE")]
    public DateTime? dateOfJoining { get; set; }
    Result:
    C# → DateTime
    DB → DATE   
    Time part is not stored at all
    API Request Example
    {
      "dateOfJoining": "2026-02-01"
    }

     */
}
