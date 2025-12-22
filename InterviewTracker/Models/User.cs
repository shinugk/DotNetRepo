using System.ComponentModel.DataAnnotations;

namespace InterviewTracker.Models
{
    public class User
    {
        [Key]
        public int id { get; set; }

        [Required]
        [MaxLength(100)]
        public string googleId { get; set; }     // "sub" claim from Google

        [MaxLength(300)]
        public string? profilePictureUrl { get; set; }

        [Required]
        [MaxLength(100)]
        public string name { get; set; }

        [Required]
        [EmailAddress]                              //validates email format
        public string email { get; set; }           // to enforce unique constraint should be done in flunet api

        [Range(18, 120)]                            //Range Works with numeric types (int, decimal, double)
        public int age { get; set; }

        [MaxLength(15)]                             //MaxLength is only for strings and arrays, not numbers.
        [Phone]                                     //validates phone format
        public string phoneNumber { get; set; }

        [MaxLength(500)]
        public string? skills {  get; set; }        // comma-separated or separate table if needed

        [MaxLength(120)]
        public string? currentCompany { get; set; }

        public byte[] resume {  get; set; }             //You can store file path or byte[]



        // 🔗 Navigation: One User → Many Employers
        public ICollection<Employer> employers { get; set; }  //(child collection)
    }
}

/*
 WHY AUTO-PROPERTIES ({ GET; SET; }) LOOK EMPTY
--------------------------------------------------------------------
Because C# automatically creates the underlying private variable:
public string Name { get; set; }
becomes:
private string _Name;
public string Name
{
    get => _Name;
    set => _Name = value;
}
So it's not empty — C# just hides the boilerplate.




NAVIGATION PROPERTY:
Here:
User.Employers → navigation property to many Employers
Employer.User → navigation property to one User

🔍 Why is it called "Navigation" property?
------------------------------------------
Because you can navigate the relationship like this:
var user = db.Users.Include(u => u.Employers).First();

Now you can access:
user.Employers   // list of related employers

Or navigate reverse:
var employer = db.Employers.Include(e => e.User).First();
employer.User    // the user who owns this employer entry

✅ Types of Navigation Properties
---------------------------------------
1️) Reference Navigation Property (single object)
Used in One-to-One or Many-to-One relationships.
public User User { get; set; }

2️) Collection Navigation Property (multiple objects)
Used in One-to-Many or Many-to-Many relationships.
public ICollection<Employer> Employers { get; set; }

📌 Why Navigation Properties Are Important?
---------------------------------------------------
| Benefit               | Explanation                                                 |
| --------------------- | ----------------------------------------------------------- |
| Load related data     | Using `.Include()` you load related entities easily         |
| Automatic joins       | EF Core automatically joins tables for you                  |
| Cascade delete        | Deleting a parent can delete children based on relationship |
| Cleaner code          | You work with objects, not manual SQL                       |





-----------------------Virtual---------------------------  (For LAZY LOADING in EF)
public virtual Container sourceContainer { get; set; } 
----------------------------------------------------------
In C#, a property like is declared virtual in a model primarily to enable Polymorphism and Dependency Injection,
allowing the property's behavior to be overridden by derived classes or intercepted by frameworks like Entity Framework (EF) Core for LAZY LOADING. 
Ex:
This proxy enables lazy loading, meaning: 
- The related sourceContainer data isn't loaded from the database immediately when you load the main entity object.
- The data is only automatically fetched from the database the first time you attempt to access the sourceContainer property (e.g., myObject.sourceContainer)
 */

