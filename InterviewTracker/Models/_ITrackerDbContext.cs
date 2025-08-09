using Microsoft.EntityFrameworkCore;

namespace InterviewTracker.Models
{
    public class ITrackerDbContext: DbContext
    {
        public ITrackerDbContext(DbContextOptions<ITrackerDbContext> options) : base(options) { }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
           
        //}

        DbSet<Names> Names { get; set; }
    }
}
