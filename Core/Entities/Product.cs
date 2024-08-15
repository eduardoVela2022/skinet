// File path
namespace Core.Entities;

// Entity class (Is like an ORM) inherits BaseEntity class
public class Product : BaseEntity
{
    // Properties
    public required string Name { get; set; }
    public required string Description { get; set; }
    public decimal Price { get; set; }
    public required string PictureUrl { get; set; }
    public required string Type { get; set; }
    public required string Brand { get; set; }
    public int QuantityInStock { get; set; }
}
