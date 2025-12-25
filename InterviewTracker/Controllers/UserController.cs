using InterviewTracker.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace InterviewTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ITrackerDbContext _dbcontext;

        public UserController(ITrackerDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        // GET: api/user/me
        [Authorize]
        [HttpGet("me")]
        public async Task<ActionResult<User>> GetCurrentUser()
        {
            //Get email from JWT / Google claims
            var email = User.FindFirstValue(ClaimTypes.Email);

            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized("User email not found in token");
            }

            var user = await _dbcontext.Users
                .Include(u => u.employers) // optional
                .FirstOrDefaultAsync(u => u.email == email);

            if (user == null)
            {
                return NotFound("User not found");
            }

            return Ok(user);
        }
    }
}
