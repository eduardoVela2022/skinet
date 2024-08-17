// Imports
using Core.Entities;

// File path
namespace Core.Specifications;

// This creates the functionality to get a list of types from the product models
public class TypeListSpecification : BaseSpecification<Product, string>
{
    public TypeListSpecification()
    {
        AddSelect(x => x.Type);
        ApplyDistinct();
    }
}
