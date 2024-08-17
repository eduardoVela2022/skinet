// Imports
using Core.Entities;

// File path
namespace Core.Specifications;

// Creates the specs of a product entity
public class ProductSpecification : BaseSpecification<Product>
{
    // Constructor for the brand and type filters
    public ProductSpecification(string? brand, string? type, string? sort) : base( x =>
        (string.IsNullOrWhiteSpace(brand) || x.Brand == brand) &&
        (string.IsNullOrWhiteSpace(type) || x.Type == type)
    )
    {
        // Switch statement for the sort filters
        switch (sort)
        {
            // Price ascending
            case "priceAsc":
                AddOrderBy(x => x.Price);
                break;
            // Price descending
            case "priceDesc":
                AddOrderByDescending(x => x.Price);
                break;
            // Name alphabetically
            default:
                AddOrderBy(x => x.Name);
                break;
        }
    }
}
