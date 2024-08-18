// Imports
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

//File path
namespace Infrastructure.Data;

// A repository implements the logic of the methods of an interface (The store context is added via a primary constructor)
public class ProductRepository(StoreContext context) : IProductRepository
{
    // Query to add a product
    public void AddProduct(Product product)
    {
        context.Products.Add(product);
    }

    // Query to delete a product
    public void DeleteProduct(Product product)
    {
        context.Products.Remove(product);
    }

    // Query to get all product brands
    public async Task<IReadOnlyList<string>> GetBrandsAsync()
    {
        // Selects the brand attribute of the product model and returns distint brands
        return await context.Products.Select(x => x.Brand).Distinct().ToListAsync();
    }

    // Query to get a single product by id
    public async Task<Product?> GetProductByIdAsync(int id)
    {
        return await context.Products.FindAsync(id);
    }

    // Query to get all products (Optionaly you can filter them by brand or type)
    public async Task<IReadOnlyList<Product>> GetProductsAsync(
        string? brand,
        string? type,
        string? sort
    )
    {
        // Builds a query
        var query = context.Products.AsQueryable();

        // Adds the brand parameter to the query if it exists
        if (!string.IsNullOrWhiteSpace(brand))
            query = query.Where(x => x.Brand == brand);

        // Adds the type parameter to the query if it exists
        if (!string.IsNullOrWhiteSpace(type))
            query = query.Where(x => x.Type == type);

        // Sorts the results depending on the type of sorting parameter passed
        query = sort switch
        {
            "priceAsc" => query.OrderBy(x => x.Price),
            "priceDesc" => query.OrderByDescending(x => x.Price),
            _ => query.OrderBy(x => x.Name),
        };

        return await query.ToListAsync();
    }

    // Query to get all product types
    public async Task<IReadOnlyList<string>> GetTypesAsync()
    {
        // Selects the type attribute of the product model and returns distint types
        return await context.Products.Select(x => x.Type).Distinct().ToListAsync();
    }

    // Query to check if a query exists
    public bool ProductExists(int id)
    {
        return context.Products.Any(x => x.Id == id);
    }

    // Check if database was changed
    public async Task<bool> SavaChangesAsync()
    {
        return await context.SaveChangesAsync() > 0;
    }

    // Query to update a product
    public void UpdateProduct(Product product)
    {
        context.Entry(product).State = EntityState.Modified;
    }
}
