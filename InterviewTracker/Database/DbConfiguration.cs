using InterviewTracker.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace InterviewTracker.Database
{
    public static class DbConfiguration
    {
        public static void ConfigureDatabase(this IServiceCollection services, IConfiguration config, bool isDev)
        {
            bool useInMemoryDb = Convert.ToBoolean(config.GetSection("DB:ITracker_InMemory_DB").Value);

            if(useInMemoryDb)
            {
                //Configuring In-Memory database 
                services.AddDbContext<ITrackerDbContext>(opt =>
                {
                    opt.UseInMemoryDatabase("ITrackerContext");
                });
            }
            else
            {
                string connectionString = config.GetSection("DB:ITracker_Connection_String").Value;
                var serverVersion = ServerVersion.AutoDetect(connectionString);

                //Configuring Disk Database
                services.AddDbContext<ITrackerDbContext>(opt =>
                {
                    opt.UseMySql(connectionString, serverVersion, mariaDbConfig =>
                    {
                        //Allows us to perform case-insensitive comparisons in linq-to-sql queries
                        mariaDbConfig.EnableStringComparisonTranslations();
                    });

                    bool enableVerboseLogging = bool.Parse(config.GetSection("DB:VERBOSE_LOGGING").Value);
                    if (enableVerboseLogging)
                    {
                        // The following three options help with debugging, but should
                        // be changed or removed for production.
                        opt.LogTo(Console.WriteLine, LogLevel.Information);
                        opt.EnableSensitiveDataLogging();
                        opt.EnableDetailedErrors();
                    }
                });
            }
        }
    }
}
