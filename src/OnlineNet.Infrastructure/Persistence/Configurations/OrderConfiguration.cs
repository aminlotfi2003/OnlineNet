using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineNet.Domain.Orders;

namespace OnlineNet.Infrastructure.Persistence.Configurations;

public sealed class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders");

        builder.HasKey(o => o.Id);

        builder.Property(o => o.RowVersion)
               .IsRowVersion()
               .IsConcurrencyToken();

        builder.Property(o => o.CreatedOn)
               .HasDefaultValueSql("SYSDATETIMEOFFSET()");

        builder.Property(o => o.Status)
               .HasConversion<string>()
               .HasMaxLength(20)
               .IsRequired();

        builder.Property(o => o.CustomerId)
               .IsRequired();

        builder.OwnsOne(o => o.Total, money =>
        {
            money.Property(m => m.Amount)
                 .HasColumnName("TotalAmount")
                 .HasPrecision(18, 2)
                 .IsRequired();

            money.Property(m => m.Currency)
                 .HasColumnName("TotalCurrency")
                 .HasMaxLength(3)
                 .IsRequired();
        });

        builder.OwnsMany(o => o.Lines, lines =>
        {
            lines.ToTable("OrderLines");
            lines.WithOwner().HasForeignKey("OrderId");

            lines.Property<Guid>("Id")
                 .ValueGeneratedOnAdd();

            lines.HasKey("Id");

            lines.Property(l => l.ProductId)
                 .IsRequired();

            lines.Property(l => l.ProductName)
                 .HasMaxLength(200)
                 .IsRequired();

            lines.Property(l => l.Quantity)
                 .IsRequired();

            lines.OwnsOne(l => l.UnitPrice, money =>
            {
                money.Property(m => m.Amount)
                     .HasColumnName("UnitPriceAmount")
                     .HasPrecision(18, 2)
                     .IsRequired();

                money.Property(m => m.Currency)
                     .HasColumnName("UnitPriceCurrency")
                     .HasMaxLength(3)
                     .IsRequired();
            });
        });

        builder.Metadata.FindNavigation(nameof(Order.Lines))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}
