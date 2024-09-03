// Imports
using Core.Entities;
using Infrastructure.Config;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

// File path
namespace Infrastructure.Data;

// Database connection (Is a Service, go to Program.cs)
public class StoreContext(DbContextOptions options) : IdentityDbContext<AppUser>(options)
{
    // Database tables
    public DbSet<Product> Products { get; set; }

    public DbSet<Address> Addresses { get; set; }

    // Specifies the attributes of model properties
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductConfiguration).Assembly);
    }
}
