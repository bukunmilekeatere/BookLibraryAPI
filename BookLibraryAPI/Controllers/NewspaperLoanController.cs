using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BookLibraryAPI.Models;

namespace BookLibraryAPI.Controllers
{
    [Route("api/newspaperloan")]
    [ApiController]
    public class NewspaperLoanController : ControllerBase
    {
        private readonly BookLibraryAPIDbContext _context;

        public NewspaperLoanController(BookLibraryAPIDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "User")]
        [HttpPost("loan/{newspaperId}")]
        public async Task<IActionResult> Loan(int newspaperId)
        {
            var newspaper = await _context.Newspapers.FindAsync(newspaperId);
            if (newspaper == null) return NotFound("Newspaper not found.");

            var id = User.Identity?.Name;
            if (_context.Loans.Any(l => l.UserId == id && l.NewspaperId == newspaperId))
                return BadRequest("User has already loaned this newspaper.");

            var loan = new Loan
            {
                UserId = id,
                NewspaperId = newspaperId
            };

            _context.Loans.Add(loan);
            await _context.SaveChangesAsync();

            return Ok("Newspaper loaned successfully.");
        }

        [Authorize(Roles = "User")]
        [HttpPost("return/{newspaperId}")]
        public async Task<IActionResult> Return(int newspaperId)
        {
            var id = User.Identity?.Name;
            var loan = _context.Loans.FirstOrDefault(l => l.UserId == id && l.NewspaperId == newspaperId);

            if (loan == null) return BadRequest("No loan record found for this book and user.");

            _context.Loans.Remove(loan);
            await _context.SaveChangesAsync();

            return Ok("Book returned successfully.");
        }
    }
}
