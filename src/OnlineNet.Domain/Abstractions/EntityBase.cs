namespace OnlineNet.Domain.Abstractions;

public abstract class EntityBase : IEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTimeOffset CreatedOn { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? ModifiedOn { get; set; }
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();

    protected void Touch() => ModifiedOn = DateTimeOffset.UtcNow;
}
