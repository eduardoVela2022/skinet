// Imports
using API.Middleware;
using API.SignalR;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

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
// Entity framwork services
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Signal R
builder.Services.AddSignalR();

// Enables CORS
builder.Services.AddCors();

// Add redis service
builder.Services.AddSingleton<IConnectionMultiplexer>(config =>
{
    var connString =
        builder.Configuration.GetConnectionString("Redis")
        ?? throw new Exception("Cannot get redis connection string");
    var configuration = ConfigurationOptions.Parse(connString, true);
    return ConnectionMultiplexer.Connect(configuration);
});

// The Redis shopping cart service
builder.Services.AddSingleton<ICartService, CartService>();

// Identity framework
builder.Services.AddAuthorization();
builder.Services.AddIdentityApiEndpoints<AppUser>().AddEntityFrameworkStores<StoreContext>();

// Stripe
builder.Services.AddScoped<IPaymentService, PaymentService>();

// Build web app
var app = builder.Build();

// [Middleware]
// Exception middleware
app.UseMiddleware<ExceptionMiddleware>();

// CORS (Specifies which origins can access the api)
app.UseCors(x =>
    x.AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials()
        .WithOrigins("http://localhost:4200", "https://localhost:4200")
);

// SignalR
app.UseAuthentication();
app.UseAuthorization();

// Ends points for controllers
app.MapControllers();

// Identity framework
app.MapGroup("api").MapIdentityApi<AppUser>(); // api/login

// SignalR
app.MapHub<NotificationHub>("hub/notifications");

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
