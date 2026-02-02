using InterviewTracker.Utils.Validation;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace InterviewTracker.Models.DTOs
{
    public class PatchEmployerDTO
    {
        [MaxLength(100)]
        public string? companyName { get; set; }

        [ValueOneOf(
            AllowedValues = $"{CompanyType.ServiceBased},{CompanyType.ProductBased},{CompanyType.Consulting}", 
            AllowNull = true)]
        public string? type { get; set; }

        [Url]
        public string? websiteLink { get; set; }

        public decimal? ctcOffered { get; set; }

        [ValueOneOf(
            AllowedValues = $"{InterviewStatus.Selected},{InterviewStatus.NotSelected},{InterviewStatus.InProgress}",
            AllowNull = true )]
        public string? interviewStatus { get; set; }

        public string? location { get; set; }

        public string? interviewLevel { get; set; }

        public string? offeredRole { get; set; }

        public string? notes { get; set; }

        public DateTime? dateOfJoining { get; set; }

        // HR updates (optional)
        public HrDetailDTO? hrDetail { get; set; }
    }
}
