using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BookLibraryAPI.Models;

namespace BookLibraryAPI.Controllers
{
    public class BookLoanController : ControllerBase
    {
        private readonly BookLibraryAPIDbContext _context;

        public BookLoanController(BookLibraryAPIDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "User")]
        [HttpPost("api/loan/{bookId}")]

        public async Task<IActionResult> Loan(int bookId)
        {
            var book = await _context.Books.FindAsync(bookId);

            if (book == null)
            {
                return NotFound("The book was found");
            }

            var id = User.Identity?.Name;

            if (_context.Loans.Any(l => l.UserId == id && l.BookId  == bookId))
            {
                return BadRequest("User has already loaned this book");
            }

            var loan = new Loan
            {
                UserId = id,
                BookId = bookId
            };

            _context.Loans.Add(loan);
            await _context.SaveChangesAsync();

            return Ok("Successful");
        }
    }
}
