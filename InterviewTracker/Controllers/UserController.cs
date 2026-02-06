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



        // POST: api/user/me/resume/upload
        [Authorize]
        [HttpPost("me/resume/upload")]
        public async Task<IActionResult> UploadMyResume(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File is required.");

            if (Path.GetExtension(file.FileName).ToLower() != ".pdf")
                return BadRequest("Only PDF files are allowed.");

            var email = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(email))
                return Unauthorized();

            var user = await _dbcontext.Users.FirstOrDefaultAsync(u => u.email == email);
            if (user == null)
                return NotFound("User not found");

            using var ms = new MemoryStream();
            await file.CopyToAsync(ms);

            user.resume = ms.ToArray();
            //user.resumeFileName = file.FileName;

            await _dbcontext.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/user/me/resume/download
        [Authorize]
        [HttpGet("me/resume/download")]
        public async Task<IActionResult> DownloadMyResume()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(email))
                return Unauthorized();

            var user = await _dbcontext.Users.FirstOrDefaultAsync(u => u.email == email);
            if (user == null)
                return NotFound("User not found");

            if (user.resume == null)
                return NotFound("Resume not uploaded");

            return File(
                user.resume,
                "application/pdf",
                /*user.resumeFileName ??*/ "resume.pdf"
            );
        }

    }
}
