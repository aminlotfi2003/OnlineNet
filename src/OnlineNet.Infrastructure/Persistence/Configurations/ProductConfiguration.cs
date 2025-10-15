using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineNet.Domain.Catalog.Products;

namespace OnlineNet.Infrastructure.Persistence.Configurations;

public sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
               .IsRequired()
               .HasMaxLength(200);

        builder.Property(p => p.Description)
               .HasMaxLength(2000);

        builder.Property(p => p.StockQuantity)
               .IsRequired();

        builder.Property(p => p.IsActive)
               .IsRequired();

        // Concurrency token (SQL Server rowversion)
        builder.Property(p => p.RowVersion)
               .IsRowVersion()
               .IsConcurrencyToken();

        // Timestamps (optional: default value on insert)
        builder.Property(p => p.CreatedOn)
               .HasDefaultValueSql("SYSDATETIMEOFFSET()");

        // Owned Value Objects
        builder.OwnsOne(p => p.Sku, sku =>
        {
            sku.Property(x => x.Value)
               .HasColumnName("Sku")
               .HasMaxLength(20)
               .IsRequired();

            // Unique index on SKU
            sku.HasIndex(x => x.Value).IsUnique();
        });

        builder.OwnsOne(p => p.Price, money =>
        {
            money.Property(x => x.Amount)
                 .HasColumnName("PriceAmount")
                 .HasPrecision(18, 2)        // decimal(18,2) for SQL Server
                 .IsRequired();

            money.Property(x => x.Currency)
                 .HasColumnName("PriceCurrency")
                 .HasMaxLength(3)
                 .IsRequired();
        });
    }
}
