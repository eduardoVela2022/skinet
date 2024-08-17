// Imports
using Core.Interfaces;
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
// Repository (Lifetime => http request life time)
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

// Build web app
var app = builder.Build();

// [Middleware]
// Ends points for controllers
app.MapControllers();

//[Start up]
try
{
    // Any resource that uses scope will be terminated at the end of this variable's lifetime
    // This creates a scoped service
    using var scope = app.Services.CreateScope();
    // Used to resolve dependencies
    var services = scope.ServiceProvider;
    // Get the store context
    var context = services.GetRequiredService<StoreContext>();
    // Apply migrations if needed
    await context.Database.MigrateAsync();
    // Adds seeds
    await StoreContextSeed.SeedAsync(context);
}
catch (Exception ex)
{
    Console.WriteLine(ex);
    throw;
}

// Run web app
app.Run();
