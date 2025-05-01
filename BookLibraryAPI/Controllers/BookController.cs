using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BookLibraryAPI.Models;
using Microsoft.VisualBasic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace BookLibraryAPI.Controllers
{
    [ApiController]
    [Route("api/books")]
    public class BookController : ControllerBase
    {
        private readonly BookLibraryAPIDbContext _context;
        private readonly IMemoryCache _cache;

        public BookController(BookLibraryAPIDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        [Authorize(Roles = "Librarian")]
        [HttpPost("logbooks")]
        // create books = log books (logging books into library)
        public async Task<IActionResult> LogBooks([FromBody] LogBookDto model)
        {
            var book = new Books
            {
                Title = model.Title,
                Genre = model.Genre,
                AuthorId = model.AuthorId, //links the book to an author
                DueDate = model.DueDate,
                LibrarianId = User.Identity?.Name
            };
            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return Ok("The book was successfully logged");
        }

        [HttpGet("getbooks")]
        public IActionResult GetBooks()
        {
            var book = _context.Books.ToList();
            return Ok(book);
        }


        [Authorize(Roles = "Librarian")]
        [HttpPut("update/{id}")]

        public async Task<IActionResult> UpdateBook(int id, [FromBody] LogBookDto model)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            book.Title = model.Title;
            book.Genre = model.Genre;
            book.AuthorId = model.AuthorId;
            book.DueDate = model.DueDate;
            book.CheckedOut = model.CheckedOut;
            book.PageCount = model.PageCount;

            await _context.SaveChangesAsync();
            return Ok("The book was successfully updated");
        }

        [Authorize(Roles ="Librarian")]
        [HttpDelete("delete/{id}")]
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

        //combines in-memory caching and pagination
        //Find books by title, author, or genre with paginated results.  
        [HttpGet("search")]
        public async Task<IActionResult> GetBooksBySearch(
            string? title, string? author, string? genre, int page = 1, int pageSize = 10)
        {
            if (page <= 0 || pageSize <= 0)
            {
                return BadRequest("Page and pageSize must be greater than 0.");
            }

            // Try to get from cache
            if (!_cache.TryGetValue("Books", out List<Books> allBooks))
            {
                allBooks = await _context.Books.Include(b => b.Author).ToListAsync();
                var options = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(30));
                _cache.Set("Books", allBooks, options);
            }

            var filtered = allBooks;

            // Apply filters
            if (!string.IsNullOrWhiteSpace(title))
                filtered = filtered.Where(b => b.Title.Contains(title)).ToList();

            if (!string.IsNullOrWhiteSpace(author))
                filtered = filtered.Where(b => b.Author.Name.Contains(author)).ToList();

            if (!string.IsNullOrWhiteSpace(genre))
                filtered = filtered.Where(b => b.Genre.Contains(genre)).ToList();

            // Pagination
            var totalBooks = filtered.Count;
            var pagedBooks = filtered
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return Ok(new
            {
                Page = page,
                PageSize = pageSize,
                TotalBooks = totalBooks,
                Data = pagedBooks
            });
        }

    }
}
