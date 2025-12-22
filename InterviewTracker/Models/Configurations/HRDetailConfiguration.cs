using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InterviewTracker.Models.Configurations
{
    public class HRDetailConfiguration : IEntityTypeConfiguration<HRDetail>
    {
        public void Configure(EntityTypeBuilder<HRDetail> builder)
        {
            builder.ToTable("HRDetails");

            builder.HasKey(hr => hr.id);

            builder.Property(hr => hr.name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(hr => hr.phoneNumber)
                   .HasMaxLength(15);

            builder.Property(hr => hr.emailId)
                   .HasMaxLength(150);

            builder.HasIndex(hr => hr.emailId);
        }
    }
}
