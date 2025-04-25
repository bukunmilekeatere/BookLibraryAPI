using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BookLibraryAPI.Models;

namespace BookLibraryAPI.Controllers
{
    [ApiController]
    public class NewspaperController : ControllerBase
    {
        private readonly BookLibraryAPIDbContext _context;

        public NewspaperController(BookLibraryAPIDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Librarian")]
        [HttpPost("api/newspapers")]

        // create books = log books (logging books into library)
        public async Task<IActionResult> LogNewspapers([FromBody] Newspapers model)
        {
            var newspaper = new Newspapers
            {
                Title = model.Title,
                AuthorName = model.AuthorName,
                DueDate = model.DueDate,
                LibrarianId = User.Identity?.Name
            };
            _context.Newspapers.Add(newspaper);
            await _context.SaveChangesAsync();

            return Ok("The newspaper was successfully logged");
        }

        [HttpGet("api/newspapers")]
        public IActionResult GetNewspapers()
        {
            var newspaper = _context.Newspapers.ToList();
            return Ok(newspaper);
        }

        [Authorize(Roles = "Librarian")]
        [HttpPut("api/newspapers/{id}")]

        public async Task<IActionResult> UpdateNewspapers(int id, [FromBody] Newspapers model)
        {
            var newspaper = await _context.Newspapers.FindAsync(id);
            if (newspaper == null)
            {
                return NotFound();
            }

            newspaper.Title = model.Title;
            newspaper.Genre = model.Genre;
            newspaper.AuthorName = model.AuthorName;
            newspaper.PageCount = model.PageCount;
            newspaper.DueDate = model.DueDate;
            newspaper.CheckedOut = model.CheckedOut;
            newspaper.SerialNumber = model.SerialNumber;

            await _context.SaveChangesAsync();
            return Ok("The newspaper was successfully updated");
        }

        [Authorize(Roles = "Librarian")]
        [HttpDelete("api/newspapers/{id}")]
        public async Task<IActionResult> DeleteNewspapers(int id)
        {
            var newspaper = await _context.Newspapers.FindAsync(id);
            if (newspaper == null)
            {
                return NotFound();
            }

            _context.Newspapers.Remove(newspaper);
            await _context.SaveChangesAsync();
            return Ok("The newspaper was successfully deleted");
        }
    }
}
