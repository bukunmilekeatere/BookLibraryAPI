using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BookLibraryAPI.Models;
using BookLibraryAPI.ModelDto;

namespace BookLibraryAPI.Controllers
{
    [ApiController]
    [Route("api/magazines")]
    public class MagazineController : ControllerBase
    {
        private readonly BookLibraryAPIDbContext _context;

        public MagazineController(BookLibraryAPIDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Librarian")]
        [HttpPost("logmagazines")]
        // create books = log magazines (logging magazines into library)
        public async Task<IActionResult> LogMagazines([FromBody] MagazineDto model)
        {
            var magazine = new Magazine
            {
                Title = model.Title,
                AuthorName = model.AuthorName,
                DueDate = model.DueDate,
                Issue = model.Issue,
                LibrarianId = User.Identity?.Name
            };
            _context.Magazines.Add(magazine);
            await _context.SaveChangesAsync();

            return Ok("The magazine was successfully logged");
        }

        [HttpGet("getmagazines")]
        public IActionResult GetMagazines()
        {
            var magazine = _context.Magazines.ToList();
            return Ok(magazine);
        }

        [Authorize(Roles = "Librarian")]
        [HttpPut("update/{id}")]

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
        [HttpDelete("delete/{id}")]
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
