using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BookLibraryAPI;
using BookLibraryAPI.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace BookLibraryAPI  // 👈 Make sure your namespace matches your project name
{
    public class Program
    {
        public static async Task Main(string[] args)  // 👈 static async Task Main!
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();

            // Register the Swagger services
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Configure the database context
            builder.Services.AddDbContext<BookLibraryAPIDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Configure Identity services
            builder.Services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<BookLibraryAPIDbContext>()
                .AddDefaultTokenProviders();

            var app = builder.Build();

            // Apply migrations and seed the database on startup
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<BookLibraryAPIDbContext>();

                // Apply any pending migrations and create the database
                context.Database.Migrate();

                // Call the static method directly using the class name
                await SeedData.InitializeAsync(services);
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();

            app.MapControllers();
            
            app.Run();
        }
    }
}
