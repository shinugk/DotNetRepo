using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace InterviewTracker.Models
{
    public class HRDetail
    {
        [Key]
        public int id { get; set; }

        [Required]
        public string name { get; set; }

        [Phone]
        public string? phoneNumber { get; set; }

        [EmailAddress]
        public string? emailId { get; set; }

        // 🔗 Foreign Key
        [ForeignKey("employer")]                 //not needed EF automatically detect by checking navigation property
        public int employerId { get; set; }

        //Navigation Property (parent reference)
        [JsonIgnore]                                    //for preventing back reference causing Infinity loop
        public Employer employer { get; set; } 
    }
}


/*
❓ Why Data Annotations don’t support UNIQUE?
-----------------------------------------------
    Data Annotations are meant for:
    - Validation
    - UI hints
    - Simple rules
    Unique constraints are:
    - Database-level concerns
    - May involve multiple columns
    - Require indexes
So EF Core intentionally keeps this in Fluent API.



 */
