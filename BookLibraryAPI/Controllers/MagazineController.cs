using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BookLibraryAPI.Models;

namespace BookLibraryAPI.Controllers
{
    [ApiController]
    public class MagazineController : ControllerBase
    {
        private readonly BookLibraryAPIDbContext _context;

        public MagazineController(BookLibraryAPIDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Librarian")]
        [HttpPost("api/magazines")]

        // create books = log books (logging books into library)
        public async Task<IActionResult> LogMagazines([FromBody] Magazine model)
        {
            var magazine = new Magazine
            {
                Title = model.Title,
                AuthorName = model.AuthorName,
                DueDate = model.DueDate,
                LibrarianId = User.Identity?.Name
            };
            _context.Magazines.Add(magazine);
            await _context.SaveChangesAsync();

            return Ok("The magazine was successfully logged");
        }

        [HttpGet("api/magazines")]
        public IActionResult GetMagazines()
        {
            var magazine = _context.Magazines.ToList();
            return Ok(magazine);
        }

        [Authorize(Roles = "Librarian")]
        [HttpPut("api/magazines/{id}")]

        public async Task<IActionResult> UpdateMagazines(int id, [FromBody] Magazine model)
        {
            var magazine = await _context.Magazines.FindAsync(id);
            if (magazine == null)
            {
                return NotFound();
            }

            magazine.Title = model.Title;
            magazine.Distributor = model.Distributor;
            magazine.AuthorName = model.AuthorName;
            magazine.DueDate = model.DueDate;
            magazine.CheckedOut = model.CheckedOut;
            magazine.SerialNumber = model.SerialNumber;

            await _context.SaveChangesAsync();
            return Ok("The magazine was successfully updated");
        }

        [Authorize(Roles = "Librarian")]
        [HttpDelete("api/magazines/{id}")]
        public async Task<IActionResult> DeleteMagazines(int id)
        {
            var magazine = await _context.Magazines.FindAsync(id);
            if (magazine == null)
            {
                return NotFound();
            }

            _context.Magazines.Remove(magazine);
            await _context.SaveChangesAsync();
            return Ok("The magazine was successfully deleted");
        }
    }
}
