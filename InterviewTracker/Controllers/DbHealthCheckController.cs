using InterviewTracker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InterviewTracker.Controllers
{

    [AllowAnonymous]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class DbHealthCheckController : ControllerBase
    {
        private readonly ILogger<DbHealthCheckController> _logger;
        private readonly ITrackerDbContext _db;

        public DbHealthCheckController(ILogger<DbHealthCheckController> logger, ITrackerDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        [HttpGet(Name = "db")]
        public async Task<ActionResult> CheckDbHealth()
        {
            bool canConnect = await _db.Database.CanConnectAsync();
            if(canConnect)
            {
                return Ok("can connect to db");
            }
            return Ok("cannot connect to db");
        }
    }
}
