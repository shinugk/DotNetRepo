using InterviewTracker.Models.Configurations;
using Microsoft.EntityFrameworkCore;

namespace InterviewTracker.Models
{
    public class ITrackerDbContext: DbContext
    {
        private readonly ILogger<ITrackerDbContext> _logger;

        public ITrackerDbContext(DbContextOptions<ITrackerDbContext> options, ILogger<ITrackerDbContext> logger = null) : base(options) 
        {
            _logger = logger;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //CLEAN OPTION TO USE IEntityTypeConfiguration<T> using FluentAPI
            //modelBuilder.ApplyConfiguration(new UserConfiguration());
            //modelBuilder.ApplyConfiguration(new EmployerConfiguration());
            //modelBuilder.ApplyConfiguration(new HRDetailConfiguration());


            //FLUENT API : Direct configure here
            /* ---------------- USER ---------------- */
            // Unique Google user
            modelBuilder.Entity<User>()
                .HasIndex(u => u.googleId)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.email)
                .IsUnique();

            modelBuilder.Entity<Note>()
                .HasOne(n => n.user)
                .WithMany(u => u.notes)
                .HasForeignKey(n => n.userId)
                .OnDelete(DeleteBehavior.Cascade);   //When the parent(User) is deleted, all related child records(Notes) are automatically deleted.


            /* ---------------- USER -> EMPLOYER (One-to-Many) ---------------- */
            modelBuilder.Entity<Employer>()
                .HasOne(e => e.user)
                .WithMany(u => u.employers)
                .HasForeignKey(e => e.userId)
                .OnDelete(DeleteBehavior.Cascade);


            /* ---------------- EMPLOYER -> HRDETAIL (One-to-One) ---------------- */
            modelBuilder.Entity<Employer>()
                .HasOne(e => e.hrDetail)
                .WithOne(h => h.employer)
                .HasForeignKey<HRDetail>(h => h.employerId)
                .OnDelete(DeleteBehavior.Cascade);


            /* ---------------- COLUMN TYPES ---------------- */
            modelBuilder.Entity<Employer>()
                .Property(e => e.offerLetter)
                .HasColumnType("longblob");

            modelBuilder.Entity<User>()
                .Property(u => u.resume)
                .HasColumnType("longblob");

        }


        //DEFINE COLUMN NAMES HERE --> THIS TAKES PRIORITY
        public DbSet<Names> Names { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Employer> Employers { get; set; }
        public DbSet<HRDetail> HrDetails { get; set; }
        public DbSet<Note> Notes { get; set; }
    }
}


/*
 Why Fluent API Is Mandatory Here:
----------------------------------------------------------
| Requirement               | Annotation?  | Fluent API   |
| ------------------------- | -----------  | ----------   |
| Unique Email              | No           | Yes          |
| Unique Phone              | No           | Yes          |
| One-to-One mapping        | No           | Yes          |
| Cascade delete            | No           | Yes          |
| Check constraints         | No           | Yes          |
| DB column type (longblob) | No           | Yes          |


Rule of Thumb:
------------------------------------------
- Use Data Annotations for validation & basic mapping
- Use Fluent API for relationships, constraints, indexes & DB behavior
 
 */