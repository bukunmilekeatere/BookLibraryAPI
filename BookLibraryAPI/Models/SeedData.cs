using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using BookLibraryAPI.Models;

namespace BookLibraryAPI
{  
    public static class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var services = scope.ServiceProvider;

                var context = services.GetRequiredService<BookLibraryAPIDbContext>();
                var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                context.Database.Migrate();

                // Seed Roles
                string[] roles = new[] { "User", "Librarian" };
                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }
                //seed librarian user
                if (await userManager.FindByEmailAsync("librarian@example.com") == null)
                {
                    var librarian = new IdentityUser
                    {
                        UserName = "librarian@example.com",
                        Email = "librarian@example.com",
                        EmailConfirmed = true
                    };
                    await userManager.CreateAsync(librarian, "Librarian123!");
                    await userManager.AddToRoleAsync(librarian, "Librarian");
                }

                // Seed Normal User
                if (await userManager.FindByEmailAsync("user@example.com") == null)
                {
                    var user = new IdentityUser
                    {
                        UserName = "user@example.com",
                        Email = "user@example.com",
                        EmailConfirmed = true
                    };
                    await userManager.CreateAsync(user, "User123!");
                    await userManager.AddToRoleAsync(user, "User");
                }

                // Seed Authors first
                if (!context.Authors.Any())
                {
                    context.Authors.AddRange(
                        new Author { Id = 1, Name = "F. Scott Fitzgerald" },
                        new Author { Id = 2, Name = "George Orwell" }
                    );
                    await context.SaveChangesAsync(); // save so we get Author IDs
                }

                // Seed Books
                if (!context.Books.Any())
                {
                    context.Books.AddRange(
                        new Books
                        {
                            Title = "The Great Gatsby",
                            Genre = "Classic",
                            PageCount = 180,
                            CheckedOut = false,
                            DueDate = null,
                            AuthorId = 1,   // Reference Author Id
                            LibrarianId = null
                        },
                        new Books
                        {
                            Title = "1984",
                            Genre = "Dystopian",
                            PageCount = 328,
                            CheckedOut = false,
                            DueDate = null,
                            AuthorId = 2,
                            LibrarianId = null
                        }
                    );
                }

                // Seed Magazines
                if (!context.Magazines.Any())
                {
                    context.Magazines.AddRange(
                        new Magazine
                        {
                            Title = "Time",
                            Issue = "March 2025",
                            Distributor = "Time Inc.",
                            SerialNumber = 101,
                            AuthorName = "Various",
                            DueDate = null,
                            CheckedOut = false,
                            LibrarianId = null
                        },
                        new Magazine
                        {
                            Title = "National Geographic",
                            Issue = "April 2025",
                            Distributor = "NatGeo",
                            SerialNumber = 102,
                            AuthorName = "Various",
                            DueDate = null,
                            CheckedOut = false,
                            LibrarianId = null
                        }
                    );

                }
                //Seed Newspaper
                if (!context.Newspapers.Any())
                {
                    context.Newspapers.AddRange(
                        new Newspaper
                        {
                            Title = "The Daily Times",
                            Genre = "General News",
                            AuthorName = "Editorial Board",
                            SerialNumber = 201,
                            PageCount = 28,
                            DatePublished = new DateTime(2025, 3, 15),
                            DueDate = null,
                            CheckedOut = false,
                            LibrarianId = null
                        },
                        new Newspaper
                        {
                            Title = "Global Gazette",
                            Genre = "World Affairs",
                            AuthorName = "International Desk",
                            SerialNumber = 202,
                            PageCount = 32,
                            DatePublished = new DateTime(2025, 4, 1),
                            DueDate = null,
                            CheckedOut = false,
                            LibrarianId = null
                        }
                    );

                }
            }
        }
    }
}

