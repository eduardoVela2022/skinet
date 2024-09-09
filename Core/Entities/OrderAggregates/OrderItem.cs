namespace Core.Entities.OrderAggregates;

public class OrderItem : BaseEntity
{
    public ProductItemOrder ItemOrdered { get; set; } = null!;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}
