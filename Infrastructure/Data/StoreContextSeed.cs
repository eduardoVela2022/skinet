// Imports
using System.Text.Json;
using Core.Entities;

// File path
namespace Infrastructure.Data;

// Class to create databse seeds
public class StoreContextSeed
{
    public static async Task SeedAsync(StoreContext context)
    {
        // If there are no products in the database, add seed data
        if (!context.Products.Any())
        {
            // Reads the product seed data file
            var productsData = await File.ReadAllTextAsync("../Infrastructure/Data/SeedData/products.json");

            // JSON deserialize
            var products = JsonSerializer.Deserialize<List<Product>>(productsData);

            // If products are null, return
            if(products == null) return;

            // Add seed data to the database
            context.Products.AddRange(products);

            // Save database
            await context.SaveChangesAsync();
        }
    }
}
