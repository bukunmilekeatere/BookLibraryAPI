using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BookLibraryAPI;
using BookLibraryAPI.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);



builder.WebHost.UseKestrel(options => options.AddServerHeader = false);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddMemoryCache();

// Register the Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>{});

// Configure the database context
builder.Services.AddDbContext<BookLibraryAPIDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure Identity services
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<BookLibraryAPIDbContext>()
    .AddDefaultTokenProviders();



var app = builder.Build();


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
    app.UseSwagger();  // This middleware serves the Swagger JSON endpoint
    app.UseSwaggerUI(options =>
    {
        options.OAuthClientId("swagger-client");
        options.OAuthAppName("Swagger API Client");
    });  
}

// Enable authentication and authorization middleware
app.UseAuthentication();  // Enable Authentication
app.UseAuthorization();   // Enable Authorization

app.MapControllers();



app.Run();
