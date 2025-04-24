using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BookLibraryAPI.Models;


namespace BookLibraryAPI
{
    public class BookLibraryAPIDbContext : IdentityDbContext<IdentityUser>
    {
        public DbSet<Books> Books { get; set; }

        public DbSet<Magazines> Magazines { get; set; }

        public DbSet<Newspapers> Newspapers { get; set; }

        public DbSet<Loan> Loans { get; set; }

        public BookLibraryAPIDbContext(DbContextOptions<BookLibraryAPIDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
