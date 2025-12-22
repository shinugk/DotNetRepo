using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InterviewTracker.Models.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {

        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(u => u.id);

            builder.Property(u => u.name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(u => u.email)
                   .IsRequired()
                   .HasMaxLength(150);

            builder.HasIndex(u => u.email)
                   .IsUnique();

            builder.Property(u => u.phoneNumber)
                   .HasMaxLength(15);

            builder.Property(u => u.skills)
                   .HasMaxLength(500);

            builder.Property(u => u.currentCompany)
                   .HasMaxLength(150);

            builder.Property(u => u.resume)
                   .HasMaxLength(500);

            // User → Employers (1-M)
            builder.HasMany(u => u.employers)
                   .WithOne(e => e.user)
                   .HasForeignKey(e => e.userId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
