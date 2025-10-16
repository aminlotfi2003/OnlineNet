using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using OnlineNet.Domain.Abstractions;
using OnlineNet.Domain.Catalog.Products;
using System.Reflection;

namespace OnlineNet.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        foreach (var property in modelBuilder.Model
                     .GetEntityTypes()
                     .SelectMany(t => t.GetProperties())
                     .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
        {
            property.SetPrecision(18);
            property.SetScale(2);
        }

        base.OnModelCreating(modelBuilder);
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        ApplyTimestamps();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(
        bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = default)
    {
        ApplyTimestamps();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    private void ApplyTimestamps()
    {
        var utcNow = DateTimeOffset.UtcNow;

        foreach (var entry in ChangeTracker.Entries<EntityBase>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    if (entry.Property(e => e.CreatedOn).CurrentValue == default)
                        entry.Property(e => e.CreatedOn).CurrentValue = utcNow;

                    entry.Property(e => e.ModifiedOn).CurrentValue = utcNow;

                    entry.Property(e => e.CreatedOn).IsModified = false;
                    break;

                case EntityState.Modified:
                    entry.Property(e => e.CreatedOn).IsModified = false;

                    if (entry.Properties.Any(p => p.IsModified && p.Metadata.Name != nameof(EntityBase.ModifiedOn)))
                        entry.Property(e => e.ModifiedOn).CurrentValue = utcNow;
                    break;
            }
        }
    }
}
