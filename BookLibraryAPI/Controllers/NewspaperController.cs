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
    }
}
