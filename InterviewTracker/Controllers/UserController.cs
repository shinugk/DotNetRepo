using InterviewTracker.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using InterviewTracker.Models.DTOs;

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


        //PATCH: api/user/1
        [Authorize]
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] PatchUserDTO dto)
        {
            if (dto == null)
                return BadRequest("Request body is required.");

            var user = await _dbcontext.Users.FindAsync(id);
            if (user == null)
                return NotFound();

            //Apply only provided values using DTO's
            if (dto.Age.HasValue)
                user.age = dto.Age;

            if (dto.PhoneNumber != null)
                user.phoneNumber = dto.PhoneNumber;

            if (dto.Skills != null)
                user.skills = dto.Skills;

            if (dto.CurrentCompany != null)
                user.currentCompany = dto.CurrentCompany;

            if (dto.Resume != null)
                user.resume = dto.Resume;

            await _dbcontext.SaveChangesAsync();

            return Ok(user);
        }
    }
}
