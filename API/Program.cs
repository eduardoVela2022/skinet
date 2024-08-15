// Imports
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// [Services]
// Controllers
builder.Services.AddControllers();
// Database connection
builder.Services.AddDbContext<StoreContext>(opt => 
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")); // This connection is set up in the developer app settings
});

// Build web app
var app = builder.Build();

// [Middleware]
app.MapControllers();

// Run web app
app.Run();
