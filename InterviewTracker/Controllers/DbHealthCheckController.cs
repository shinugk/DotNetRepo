using InterviewTracker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InterviewTracker.Controllers
{

    //[AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class DbHealthCheckController : ControllerBase
    {
        private readonly ILogger<DbHealthCheckController> _logger;
        private readonly ITrackerDbContext _db;

        public DbHealthCheckController(ILogger<DbHealthCheckController> logger, ITrackerDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        //[Authorize]                 //<-- without adding bearer jwt token in api call it throws 401 unauthorized
        [HttpGet(Name = "db")]
        public async Task<ActionResult> CheckDbHealth()
        {
            bool canConnect = await _db.Database.CanConnectAsync();
            return Ok(new
            {
                status = canConnect ? "Can Connect to DB YAY!" : "Cannot connect to DB",
                canConnect = canConnect,
                checkedAt = DateTime.UtcNow
            });

        }
    }
}
