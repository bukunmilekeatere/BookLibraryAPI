using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BookLibraryAPI.Models;

namespace BookLibraryAPI.Controllers
{
    [ApiController]
    [Route("api/newspapers")]
    public class NewspaperController : ControllerBase
    {
        private readonly BookLibraryAPIDbContext _context;

        public NewspaperController(BookLibraryAPIDbContext context)
        {
            _context = context;
        }


        [Authorize(Roles = "Librarian")]
        [HttpPost("lognewspapers")]

        // create books = log newspaper (logging newspaper into library)
        public async Task<IActionResult> LogNewspapers([FromBody] NewspaperDto model)
        {
            var newspaper = new Newspaper
            {
                Title = model.Title,
                AuthorName = model.AuthorName,
                DueDate = model.DueDate,
                Genre = model.Genre,
                CheckedOut = model.CheckedOut,
                LibrarianId = User.Identity?.Name
            };
            _context.Newspapers.Add(newspaper);
            await _context.SaveChangesAsync();

            return Ok("The newspaper was successfully logged");
        }

        [HttpGet("getnewspapers")]
        public IActionResult GetNewspapers()
        {
            var newspaper = _context.Newspapers.ToList();
            return Ok(newspaper);
        }

        [Authorize(Roles = "Librarian")]
        [HttpPut("update/{id}")]

        public async Task<IActionResult> UpdateNewspapers(int id, [FromBody] NewspaperDto model)
        {
            var newspaper = await _context.Newspapers.FindAsync(id);
            if (newspaper == null)
            {
                return NotFound();
            }

            newspaper.Title = model.Title;
            newspaper.Genre = model.Genre;
            newspaper.AuthorName = model.AuthorName;
            newspaper.DueDate = model.DueDate;
            newspaper.CheckedOut = model.CheckedOut;

            await _context.SaveChangesAsync();
            return Ok("The newspaper was successfully updated");
        }

        [Authorize(Roles = "Librarian")]
        [HttpDelete("delete/{id}")]
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
