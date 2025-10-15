namespace OnlineNet.Domain.Abstractions;

public interface IEntity
{
    Guid Id { get; set; }
    DateTimeOffset CreatedOn { get; set; }
    DateTimeOffset? ModifiedOn { get; set; }
    byte[] RowVersion { get; set; }
}
