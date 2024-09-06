namespace Core.Entities;

// Redis shopping cart item
public class ShoppingCart
{
    public required string Id { get; set; }
    public List<CartItem> Items { get; set; } = [];
    public int? DeliveryMethod { get; set; }
    public string? ClientSecret { get; set; }
    public string? PaymentIntentId { get; set; }
}
