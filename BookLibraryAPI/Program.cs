using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BookLibraryAPI;
using BookLibraryAPI.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Register the Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid JWT token",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// Configure the database context
builder.Services.AddDbContext<BookLibraryAPIDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure Identity services
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<BookLibraryAPIDbContext>()
    .AddDefaultTokenProviders();

// Configure JWT Authentication (replace with your Auth provider details)
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(options =>
    {
        options.Authority = "https://localhost:5001";  // Replace with your authentication provider
        options.Audience = "api1"; 
        options.RequireHttpsMetadata = false;
    });


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
