using System.ComponentModel.DataAnnotations;

namespace InterviewTracker.Models.DTOs
{
    public class HrDetailDTO
    {
            [Required]
            public string name { get; set; }

            [Phone]
            public string? phoneNumber { get; set; }

            [EmailAddress]
            public string? emailId { get; set; }
    }
}
