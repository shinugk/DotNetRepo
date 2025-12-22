using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InterviewTracker.Models.Configurations
{
    public class EmployerConfiguration : IEntityTypeConfiguration<Employer>
    {

        public void Configure(EntityTypeBuilder<Employer> builder)
        {
            builder.ToTable("Employers");

            builder.HasKey(e => e.id);

            builder.Property(e => e.companyName)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(e => e.type)
                   .IsRequired()
                   .HasConversion<int>();   // enum → int

            builder.Property(e => e.websiteLink)
                   .HasMaxLength(300);

            builder.Property(e => e.offerLetter)
                   .HasMaxLength(500);

            builder.Property(e => e.ctcOffered)
                   .HasPrecision(18, 2);

            builder.Property(e => e.interviewStatus)
                   .HasMaxLength(50);

            builder.Property(e => e.location)
                   .HasMaxLength(100);

            // Employer → HRDetails (1-1)
            builder.HasOne(e => e.hrDetail)
                   .WithOne(hr => hr.employer)
                   //.HasForeignKey(hr => hr.id)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
