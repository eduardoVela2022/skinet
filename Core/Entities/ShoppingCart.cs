namespace Core.Entities;

// Redis shopping cart item
public class ShoppingCart
{
    public required string Id { get; set; }
    public List<CartItem> Items { get; set; } = [];
}
