using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineNet.Domain.Customers;

namespace OnlineNet.Infrastructure.Persistence.Configurations;

public sealed class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.RowVersion)
               .IsRowVersion()
               .IsConcurrencyToken();

        builder.Property(c => c.CreatedOn)
               .HasDefaultValueSql("SYSDATETIMEOFFSET()");

        builder.Property(c => c.ActiveBasketId)
               .IsRequired(false);

        builder.OwnsOne(c => c.Name, name =>
        {
            name.Property(n => n.FirstName)
                .HasColumnName("FirstName")
                .HasMaxLength(100)
                .IsRequired();

            name.Property(n => n.LastName)
                .HasColumnName("LastName")
                .HasMaxLength(100);
        });

        builder.OwnsOne(c => c.Email, email =>
        {
            email.Property(e => e.Value)
                 .HasColumnName("Email")
                 .HasMaxLength(320)
                 .IsRequired();

            email.HasIndex(e => e.Value)
                 .IsUnique();
        });
    }
}
