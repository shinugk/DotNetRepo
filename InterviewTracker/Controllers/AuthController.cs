using System;
using Google.Apis.Auth;
using InterviewTracker.Models;
using InterviewTracker.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace InterviewTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ITrackerDbContext _db;
        private readonly JwtTokenService _jwtService;
                       
        public AuthController(ITrackerDbContext db, JwtTokenService jwtService)
        {
            _db = db;
            _jwtService = jwtService;
        }

        [HttpPost("google")]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginRequest request)
        {
            // 1️) Validate Google ID Token
            var payload = await ValidateGoogleToken(request.IdToken);
            if (payload == null)
                return Unauthorized("Invalid Google token");

            // 2️) Find or create user
            User user;
            try
            {
                user = _db.Users.SingleOrDefault(u => u.email == payload.Email);
            }
            catch (Exception ex)
            {
                Console.WriteLine("DB error:");
                Console.WriteLine(ex);
                return StatusCode(500, "Database error");
            }

            if (user == null)
            {
                user = new User
                {
                    email = payload.Email,
                    name = payload.Name,
                    googleId = payload.Subject,
                    profilePictureUrl = payload.Picture
                };

                _db.Users.Add(user);
                await _db.SaveChangesAsync();
            }


            // 3️) Issue YOUR JWT
            var jwt = _jwtService.GenerateToken(user);

            return Ok(new
            {
                token = jwt
            });
        }

        private async Task<GoogleJsonWebSignature.Payload?> ValidateGoogleToken(string idToken)
        {
            try
            {
                return await GoogleJsonWebSignature.ValidateAsync(idToken);
            }
            catch
            {
                return null;
            }
        }
    }
}
