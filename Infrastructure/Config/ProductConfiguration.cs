// Imports
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

// File path
namespace Infrastructure.Config;

// This class uses the "Entity type configuration" interface, to specify a property of a product
// This allows us to configure more deeply a property
public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        // Specifies the price property which is a decimal, so that the migration doesn't throw an error when created
        builder.Property(x => x.Price).HasColumnType("decimal(18,2)");
    }
}
