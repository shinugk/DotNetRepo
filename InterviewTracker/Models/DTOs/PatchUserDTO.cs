namespace InterviewTracker.Models.DTOs
{
    public class PatchUserDTO
    {
        public int? Age { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Skills { get; set; }
        public string? CurrentCompany { get; set; }
        public byte[]? Resume { get; set; }
    }
}
