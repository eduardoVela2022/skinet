// Imports
using Core.Entities;

// File path
namespace Core.Specifications;

// This creates the functionality to get a list of brands from the product models
public class BrandListSpecification : BaseSpecification<Product, string>
{
    public BrandListSpecification()
    {
        AddSelect(x => x.Brand);
        ApplyDistinct();
    }
}
