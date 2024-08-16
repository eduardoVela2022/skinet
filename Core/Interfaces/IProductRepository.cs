// Imports
using Core.Entities;

// File path
namespace Core.Interfaces;

// This interface stores all the product queries
public interface IProductRepository
{
    // Query to get all products (Optionaly you can filter them by brand or type)
    Task<IReadOnlyList<Product>> GetProductsAsync(string? brand, string? type, string? sort);

    // Query to get a single product by id
    Task<Product?> GetProductByIdAsync(int id);

    // Query to get all product brands
    Task<IReadOnlyList<string>> GetBrandsAsync();

    // Query to get all product types
    Task<IReadOnlyList<string>> GetTypesAsync();

    // Query to add a product
    void AddProduct(Product product);

    // Query to update a product
    void UpdateProduct(Product product);

    // Query to delete a product
    void DeleteProduct(Product product);

    // Query to check if a query exists
    bool ProductExists(int id);

    // Check if database was changed
    Task<bool> SavaChangesAsync();
}
