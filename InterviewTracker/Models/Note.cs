using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace InterviewTracker.Models
{
    public class Note
    {
        [Key]
        public int id { get; set; }

        [Required]
        public int userId { get; set; }   // FK

        [MaxLength(150)]
        public string? title { get; set; }

        [Required]
        public string content { get; set; } // For MariaDB / MySQL, EF Core maps this to: LONGTEXT ~4GB Max size
        // Stores full note (paragraphs or bullet points)

        public DateTime createdAt { get; set; } = DateTime.UtcNow;

        // Navigation property (parent reference)
        [JsonIgnore]                 //JsonIgnore for preventing back reference & circular reference
        public User user { get; set; }
    }
}
