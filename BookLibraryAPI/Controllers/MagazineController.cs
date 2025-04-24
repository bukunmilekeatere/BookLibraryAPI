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
        public async Task<IActionResult> LogMagazines([FromBody] Magazines model)
        {
            var magazine = new Magazines
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
        public IActionResult GetBooks()
        {
            var magazine = _context.Magazines.ToList();
            return Ok(magazine);
        }
    }
}
