using Microsoft.EntityFrameworkCore;
using InterviewTracker.Models;

namespace InterviewTracker.Operations
{
    public class MigrationOperations
    {
        private readonly ITrackerDbContext _dbContext;
        private readonly IConfiguration _config;

        public MigrationOperations(ITrackerDbContext dbContext, IConfiguration config)
        {
            _dbContext = dbContext;
            _config = config;
        }

        public void Migrate()
        {
            bool UseInMemoryDb = Convert.ToBoolean(_config.GetSection("DB:ITracker_InMemory_DB").Value);
            if (!UseInMemoryDb)
            {
                _dbContext.Database.Migrate();
            }
        }
    }
}
