using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BookLibraryAPI.Models;

namespace BookLibraryAPI.Controllers
{
    [Route("api/magazineloan")]
    [ApiController]
    public class MagazineLoanController : ControllerBase
    {
        private readonly BookLibraryAPIDbContext _context;

        public MagazineLoanController(BookLibraryAPIDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "User")]
        [HttpPost("loan/{magazineId}")]
        public async Task<IActionResult> Loan(int magazineId)
        {
            var magazine = await _context.Magazines.FindAsync(magazineId);
            if (magazine == null) return NotFound("Magazine not found.");

            var id = User.Identity?.Name;
            if (_context.Loans.Any(l => l.UserId == id && l.MagazineId == magazineId))
                return BadRequest("User has already loaned this magazine.");

            var loan = new Loan
            {
                UserId = id,
                MagazineId = magazineId
            };

            _context.Loans.Add(loan);
            await _context.SaveChangesAsync();

            return Ok("Magazine loaned successfully.");
        }
    }
}
