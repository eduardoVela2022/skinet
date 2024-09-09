using Core.Entities.OrderAggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Config;

// Order config class
public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        // Order owns the shipping address and payment summary
        builder.OwnsOne(x => x.ShippingAddress, o => o.WithOwner());
        builder.OwnsOne(x => x.PaymentSummary, o => o.WithOwner());
        // Enum config
        builder
            .Property(x => x.Status)
            .HasConversion(o => o.ToString(), o => (OrderStatus)Enum.Parse(typeof(OrderStatus), o));
        builder.Property(x => x.Subtotal).HasColumnType("decimal(18,2)");
        // Order list
        builder.HasMany(x => x.OrderItems).WithOne().OnDelete(DeleteBehavior.Cascade);
        // Date conversion
        builder
            .Property(x => x.OrderDate)
            .HasConversion(
                d => d.ToUniversalTime(),
                d => DateTime.SpecifyKind(d, DateTimeKind.Utc)
            );
    }
}
