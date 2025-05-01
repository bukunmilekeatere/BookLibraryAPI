using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BookLibraryAPI.Models;
using System.Reflection.Emit;

namespace BookLibraryAPI
{
    public class BookLibraryAPIDbContext : IdentityDbContext<IdentityUser>
    {
        public DbSet<Books> Books { get; set; }
        public DbSet<Magazine> Magazines { get; set; }
        public DbSet<Newspaper> Newspapers { get; set; }
        public DbSet<Loan> Loans { get; set; }
        public DbSet<Author> Authors { get; set; }

        public BookLibraryAPIDbContext(DbContextOptions<BookLibraryAPIDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Seed Author
            builder.Entity<Author>().HasData(
                new Author { Id = 1, Name = "John Doe" },
                new Author { Id = 2, Name = "Jane Austen" }
            );



            // Seed Book with foreign key AuthorId
            builder.Entity<Books>().HasData(
                new Books
                {
                    Id = 1,
                    Title = "Pride and Prejudice",
                    Genre = "Romance",
                    PageCount = 432,
                    CheckedOut = false,
                    DueDate = null,
                    AuthorId = 2,
                    LibrarianId = "lib213"
                },
                new Books
                {
                    Id = 2,
                    Title = "C# in Depth",
                    Genre = "Programming",
                    PageCount = 900,
                    CheckedOut = false,
                    DueDate = null,
                    AuthorId = 1,
                    LibrarianId = "lib123"
                }
            );
        }
    }
}
