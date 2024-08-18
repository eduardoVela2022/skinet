// Imports
using Core.Entities;

// File path
namespace Core.Specifications;

// Creates the specs of a product entity
public class ProductSpecification : BaseSpecification<Product>
{
    // Constructor for the brand and type filters
    public ProductSpecification(ProductSpecParams specParams)
        : base(x =>
            (
                string.IsNullOrEmpty(specParams.Search)
                || x.Name.ToLower().Contains(specParams.Search)
            )
            && (specParams.Brands.Count == 0 || specParams.Brands.Contains(x.Brand))
            && (specParams.Types.Count == 0 || specParams.Types.Contains(x.Type))
        )
    {
        // Applies pagination
        ApplyPaging(specParams.PageSize * (specParams.PageIndex - 1), specParams.PageSize);

        // Switch statement for the sort filters
        switch (specParams.Sort)
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
