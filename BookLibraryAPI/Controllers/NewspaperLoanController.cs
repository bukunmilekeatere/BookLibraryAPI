using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BookLibraryAPI.Models;

namespace BookLibraryAPI.Controllers
{
    public class NewspaperLoanController : ControllerBase
    {
        private readonly BookLibraryAPIDbContext _context;

        public NewspaperLoanController(BookLibraryAPIDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "User")]
        [HttpPost("api/loan/{newspaperId}")]

        public async Task<IActionResult> Loan(int newspaperId)
        {
            var newspaper = await _context.Newspapers.FindAsync(newspaperId);

            if (newspaper == null)
            {
                return NotFound("The newspaper was found");
            }

            var id = User.Identity?.Name;

            if (_context.Loans.Any(l => l.UserId == id && l.NewspaperId == newspaperId))
            {
                return BadRequest("User has already loaned this newspaper");
            }

            var loan = new Loan
            {
                UserId = id,
                MagazineId = newspaperId
            };

            _context.Loans.Add(loan);
            await _context.SaveChangesAsync();

            return Ok("Successful");
        }
    }
}
