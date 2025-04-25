using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BookLibraryAPI.Models;
using Microsoft.VisualBasic;
using Microsoft.EntityFrameworkCore;

namespace BookLibraryAPI.Controllers
{
    [ApiController]
    [Route("api/books")]
    public class BookController : ControllerBase
    {
        private readonly BookLibraryAPIDbContext _context; 

        public BookController(BookLibraryAPIDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Librarian")]
        [HttpPost("api/books")]

        // create books = log books (logging books into library)
        public async Task<IActionResult> LogBooks([FromBody] Books model)
        {
            var book = new Books
            {
                Title = model.Title,
                Genre = model.Genre,
                AuthorName = model.AuthorName,
                DueDate = model.DueDate,
                LibrarianId = User.Identity?.Name
            };
            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return Ok("The book was successfully logged");
        }

        [HttpGet("api/books")]
        public IActionResult GetBooks()
        {
            var book = _context.Books.ToList();
            return Ok(book);
        }


        [Authorize(Roles = "Librarian")]
        [HttpPut("api/books/{id}")]

        public async Task<IActionResult> UpdateBook(int id, [FromBody] Books model)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            book.Title = model.Title;
            book.Genre = model.Genre;
            book.AuthorName = model.AuthorName;
            book.DueDate = model.DueDate;
            book.CheckedOut = model.CheckedOut;
            book.PageCount = model.PageCount;

            await _context.SaveChangesAsync();
            return Ok("The book was successfully updated");
        }

        [Authorize(Roles ="Librarian")]
        [HttpDelete("api/books/{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return Ok("The book was successfully deleted");
        }

    }
}
