using InterviewTracker.Models;
using InterviewTracker.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace InterviewTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployerController : ControllerBase
    {
        private readonly ITrackerDbContext _dbcontext;

        public EmployerController(ITrackerDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        // ============================
        // GET: api/employer
        // ============================
        [Authorize]    //-->need to add this bcoz claims comes from JWT bearer token
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employer>>> GetAllEmployersForCurrentUser()
        {
            // 1️. Get email from JWT claims
            var email = User.FindFirstValue(ClaimTypes.Email);

            if (string.IsNullOrEmpty(email))
                return Unauthorized("User email not found in token");

            // 2️. Get user with employers
            var user = await _dbcontext.Users.Include(u => u.employers)
                                             .ThenInclude(e => e.hrDetail) // optional
                                             .FirstOrDefaultAsync(u => u.email == email);

            if (user == null)
                return NotFound("User not found");

            // 3️. Return employer list
            return Ok(user.employers);
        }

        /*
         We can get any employer by id that exists in system. this does not need authorization 
         ============================
         GET: api/employer/{id}
         ============================
        */
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Employer>> GetEmployerById(int id)
        {
            var employer = await _dbcontext.Employers.Include(e => e.hrDetail)
                                                     .FirstOrDefaultAsync(e => e.id == id);

            if (employer == null)
                return NotFound();

            return Ok(employer);
        }

        // PATCH: api/employer/{id}
        [Authorize]
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateEmployerForCurrentUser(
            int id,
            [FromBody] PatchEmployerDTO dto)
        {
            if (dto == null)
                return BadRequest("Request body is required.");

            // 1️ Get logged-in user email
            var email = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(email))
                return Unauthorized("User email not found in token");

            // 2️ Fetch employer belonging to this user
            var employer = await _dbcontext.Employers
                .Include(e => e.user)
                .Include(e => e.hrDetail)
                .FirstOrDefaultAsync(e =>
                    e.id == id &&
                    e.user.email == email);

            if (employer == null)
                return NotFound("Employer not found or access denied");

            // 3️ Apply PATCH values
            if (dto.companyName != null)
                employer.companyName = dto.companyName;

            if (dto.type != null)
                employer.type = dto.type;

            if (dto.websiteLink != null)
                employer.websiteLink = dto.websiteLink;

            if (dto.ctcOffered.HasValue)
                employer.ctcOffered = dto.ctcOffered;

            if (dto.interviewStatus != null)
                employer.interviewStatus = dto.interviewStatus;

            if (dto.location != null)
                employer.location = dto.location;

            if (dto.interviewLevel != null)
                employer.interviewLevel = dto.interviewLevel;

            if (dto.offeredRole != null)
                employer.offeredRole = dto.offeredRole;

            if (dto.notes != null)
                employer.notes = dto.notes;

            if (dto.dateOfJoining != null)
                employer.dateOfJoining = dto.dateOfJoining;

            // 4️ Patch HR details (create if missing)
            if (dto.hrDetail != null)
            {
                if (employer.hrDetail == null)
                {
                    employer.hrDetail = new HRDetail();
                }

                if (dto.hrDetail.name != null)
                    employer.hrDetail.name = dto.hrDetail.name;

                if (dto.hrDetail.phoneNumber != null)
                    employer.hrDetail.phoneNumber = dto.hrDetail.phoneNumber;

                if (dto.hrDetail.emailId != null)
                    employer.hrDetail.emailId = dto.hrDetail.emailId;
            }

            await _dbcontext.SaveChangesAsync();

            return Ok(employer); // ✅ Return updated employer
        }


        // POST: api/employer
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Employer>> CreateEmployerForCurrentUser(
            [FromBody] CreateEmployerDTO employerdto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // 1️ Get logged-in user email from JWT
            var email = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(email))
                return Unauthorized("User email not found in token");

            // 2️ Get user
            var user = await _dbcontext.Users
                .FirstOrDefaultAsync(u => u.email == email);

            if (user == null)
                return NotFound("User not found");

            // 3️ Map DTO → Entity
            var employer = new Employer
            {
                companyName = employerdto.companyName,
                type = employerdto.type,
                websiteLink = employerdto.websiteLink,
                ctcOffered = employerdto.ctcOffered,
                interviewStatus = employerdto.interviewStatus,
                location = employerdto.location,
                interviewLevel = employerdto.interviewLevel,
                offeredRole = employerdto.offeredRole,
                notes = employerdto.notes,
                dateOfJoining = employerdto.dateOfJoining,
                userId = user.id
            };

            // 4️ Optional HR creation
            if (employerdto.hrDetail != null)
            {
                employer.hrDetail = new HRDetail
                {
                    name = employerdto.hrDetail.name,
                    phoneNumber = employerdto.hrDetail.phoneNumber,
                    emailId = employerdto.hrDetail.emailId
                };
            }

            _dbcontext.Employers.Add(employer);
            await _dbcontext.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetAllEmployersForCurrentUser),
                new { },
                employer
            );
        }

        // ============================
        // DELETE: api/employer/{id}
        // ============================
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployerForCurrentUser(int id)
        {
            //1.Get logged-in user's email from JWT
            var email = User.FindFirstValue(ClaimTypes.Email);

            if (string.IsNullOrEmpty(email))
                return Unauthorized("User email not found in token");

            //2️.Find employer that belongs to this user
            var employer = await _dbcontext.Employers.Include(e => e.user)
                                                     .FirstOrDefaultAsync(e =>  e.id == id && e.user.email == email);

            if (employer == null)
                return NotFound("Employer not found or access denied");

            //3️.Delete employer
            _dbcontext.Employers.Remove(employer);
            await _dbcontext.SaveChangesAsync();

            return NoContent(); // ✅ 204
        }
    }


}
