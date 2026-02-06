using InterviewTracker.Models;
using InterviewTracker.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace InterviewTracker.Controllers
{
    [Route("api/[controller]")]
    public class NoteController : ControllerBase
    {
        private readonly ITrackerDbContext _dbcontext;

        public NoteController(ITrackerDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        // 🔹 Helper: Get current user id
        private async Task<User?> GetCurrentUserAsync()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(email))
                return null;

            return await _dbcontext.Users.FirstOrDefaultAsync(u => u.email == email);
        }

        // ============================================================
        // GET: api/notes
        // Get all notes for logged-in user
        // ============================================================
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Note>>> GetMyNotes()
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
                return Unauthorized();

            var notes = await _dbcontext.Notes
                .Where(n => n.userId == user.id)
                .OrderByDescending(n => n.createdAt)
                .ToListAsync();

            return Ok(notes);
        }

        // ============================================================
        // POST: api/notes
        // Create a new note
        // ============================================================
        [HttpPost]
        public async Task<ActionResult<Note>> CreateNote([FromBody] CreateNoteDTO dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.content))
                return BadRequest("Note content is required.");

            var user = await GetCurrentUserAsync();
            if (user == null)
                return Unauthorized();

            var note = new Note
            {
                title = dto.title,
                content = dto.content,
                userId = user.id
            };

            _dbcontext.Notes.Add(note);
            await _dbcontext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMyNotes), new { id = note.id }, note);
        }

        // ============================================================
        // PUT: api/notes/{id}
        // Update an existing note
        // ============================================================
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNote(int id, [FromBody] UpdateNoteDTO dto)
        {
            if (dto == null)
                return BadRequest("Request body is required.");

            var user = await GetCurrentUserAsync();
            if (user == null)
                return Unauthorized();

            var note = await _dbcontext.Notes
                .FirstOrDefaultAsync(n => n.id == id && n.userId == user.id);

            if (note == null)
                return NotFound("Note not found.");

            if (dto.title != null)
                note.title = dto.title;

            if (dto.content != null)
                note.content = dto.content;

            await _dbcontext.SaveChangesAsync();
            return Ok(note);
        }

        // ============================================================
        // DELETE: api/notes/{id}
        // Delete a note
        // ============================================================
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNote(int id)
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
                return Unauthorized();

            var note = await _dbcontext.Notes
                .FirstOrDefaultAsync(n => n.id == id && n.userId == user.id);

            if (note == null)
                return NotFound("Note not found.");

            _dbcontext.Notes.Remove(note);
            await _dbcontext.SaveChangesAsync();

            return NoContent(); //return Ok("Note deleted successfully.");
        }
    }
}
