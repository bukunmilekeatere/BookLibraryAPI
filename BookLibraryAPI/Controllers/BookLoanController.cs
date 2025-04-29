using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BookLibraryAPI.Models;

namespace BookLibraryAPI.Controllers
{
    [Route("api/bookloan")]
    [ApiController]
    public class BookLoanController : ControllerBase
    {
        private readonly BookLibraryAPIDbContext _context;

        public BookLoanController(BookLibraryAPIDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "User")]
        [HttpPost("loan/{bookId}")]
        public async Task<IActionResult> Loan(int bookId)
        {
            var book = await _context.Books.FindAsync(bookId);
            if (book == null) return NotFound("Book not found.");

            var id = User.Identity?.Name;
            if (_context.Loans.Any(l => l.UserId == id && l.BookId == bookId))
                return BadRequest("User has already loaned this book.");

            var loan = new Loan
            {
                UserId = id,
                BookId = bookId
            };

            _context.Loans.Add(loan);
            await _context.SaveChangesAsync();

            return Ok("Book loaned successfully.");
        }

        [Authorize(Roles = "User")]
        [HttpPost("return/{bookId}")]
        public async Task<IActionResult> Return(int bookId)
        {
            var id = User.Identity?.Name;
            var loan = _context.Loans.FirstOrDefault(l => l.UserId == id && l.BookId == bookId);

            if (loan == null) return BadRequest("No loan record found for this book and user.");

            _context.Loans.Remove(loan);
            await _context.SaveChangesAsync();

            return Ok("Book returned successfully.");
        }
    }
}
