
**Data Annotations Vs FluentAPI:**

```
public class InterviewTrackerDbContext : DbContext
{
    public InterviewTrackerDbContext(DbContextOptions<InterviewTrackerDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Employer> Employers { get; set; }
    public DbSet<HRDetail> HRDetails { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new EmployerConfiguration());
        modelBuilder.ApplyConfiguration(new HRDetailConfiguration());
    }
}
```

1️⃣ builder in IEntityTypeConfiguration<T>
When you write:
```
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // 👆 THIS is the builder
        builder.ToTable("Users");
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Name)
               .IsRequired()
               .HasMaxLength(100);
    }
}
```
🔍 What exactly is it?: EntityTypeBuilder<User> builder
```
This object lets you configure:
- Table name
- Primary key
- Properties
- Relationships
- Indexes
- Constraints
```

In EF Core Fluent API, builder is an object provided by EF Core that you use to configure how an entity or its properties map to the database.

| **Purpose**         | **Data Annotation**                     | **Fluent API Equivalent**                                      | **Notes**                            |
| ------------------- | --------------------------------------- | -------------------------------------------------------------- | ------------------------------------ |
| Primary Key         | `[Key]`                                 | `builder.HasKey(e => e.Id);`                                   | Convention already treats `Id` as PK |
| Required / NOT NULL | `[Required]`                            | `builder.Property(e => e.Prop).IsRequired();`                  | Works for strings & nullable types   |
| Max Length (string) | `[MaxLength(100)]`                      | `builder.Property(e => e.Prop).HasMaxLength(100);`             | ❌ Not for numbers                    |
| Min Length          | `[MinLength(5)]`                        | ❌ No DB equivalent                                             | Validation only                      |
| String Length       | `[StringLength(100)]`                   | `builder.Property(e => e.Prop).HasMaxLength(100);`             | Min length ignored at DB             |
| Numeric Range       | `[Range(18, 65)]`                       | `builder.HasCheckConstraint("CK", "Age BETWEEN 18 AND 65");`   | Range is validation only             |
| Email Format        | `[EmailAddress]`                        | ❌ No DB equivalent                                             | Validation only                      |
| Phone Format        | `[Phone]`                               | ❌ No DB equivalent                                             | Validation only                      |
| URL Format          | `[Url]`                                 | ❌ No DB equivalent                                             | Validation only                      |
| Column Name         | `[Column("email")]`                     | `builder.Property(e => e.Prop).HasColumnName("email");`        |                                      |
| Column Type         | `[Column(TypeName="varchar(150)")]`     | `builder.Property(e => e.Prop).HasColumnType("varchar(150)");` |                                      |
| Table Name          | `[Table("Users")]`                      | `builder.ToTable("Users");`                                    |                                      |
| Ignore Property     | `[NotMapped]`                           | `builder.Ignore(e => e.Prop);`                                 |                                      |
| Foreign Key         | `[ForeignKey("User")]`                  | `builder.HasOne(...).WithMany(...).HasForeignKey(...);`        | Fluent API preferred                 |
| Index               | `[Index(nameof(Email))]`                | `builder.HasIndex(e => e.Email);`                              | EF Core 5+                           |
| Unique Index        | `[Index(nameof(Email), IsUnique=true)]` | `builder.HasIndex(e => e.Email).IsUnique();`                   | Fluent API preferred                 |
| Decimal Precision   | ❌ Not supported                         | `builder.Property(e => e.Amount).HasPrecision(18,2);`          | DB-only                              |
| Enum Mapping        | ❌ Not supported                         | `builder.Property(e => e.Type).HasConversion<int>();`          |                                      |
| Cascade Delete      | ❌ Not supported                         | `.OnDelete(DeleteBehavior.Cascade);`                           |                                      |
| Composite Key       | ❌ Not supported                         | `builder.HasKey(e => new { e.A, e.B });`                       |                                      |
| Concurrency Token   | `[ConcurrencyCheck]`                    | `builder.Property(e => e.Prop).IsConcurrencyToken();`          |                                      |
| Row Version         | `[Timestamp]`                           | `builder.Property(e => e.RowVersion).IsRowVersion();`          |                                      |

