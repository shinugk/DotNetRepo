using System.ComponentModel.DataAnnotations;

namespace InterviewTracker.Models
{
    public class Names
    {
        [Key]
        public int id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
