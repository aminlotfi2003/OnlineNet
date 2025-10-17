using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineNet.Domain.Baskets;

namespace OnlineNet.Infrastructure.Persistence.Configurations;

public sealed class BasketConfiguration : IEntityTypeConfiguration<Basket>
{
    public void Configure(EntityTypeBuilder<Basket> builder)
    {
        builder.ToTable("Baskets");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.CustomerId)
               .IsRequired();

        builder.Property(b => b.IsLocked)
               .IsRequired();

        builder.Property(b => b.RowVersion)
               .IsRowVersion()
               .IsConcurrencyToken();

        builder.Property(b => b.CreatedOn)
               .HasDefaultValueSql("SYSDATETIMEOFFSET()");

        builder.HasIndex(b => b.CustomerId)
               .IsUnique();

        builder.OwnsMany(b => b.Items, items =>
        {
            items.ToTable("BasketItems");
            items.WithOwner().HasForeignKey("BasketId");

            items.Property<Guid>("Id")
                 .ValueGeneratedOnAdd();

            items.HasKey("Id");

            items.Property(i => i.ProductId)
                 .IsRequired();

            items.Property(i => i.ProductName)
                 .HasMaxLength(200)
                 .IsRequired();

            items.Property(i => i.Quantity)
                 .IsRequired();

            items.OwnsOne(i => i.UnitPrice, money =>
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

        builder.Metadata.FindNavigation(nameof(Basket.Items))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}
