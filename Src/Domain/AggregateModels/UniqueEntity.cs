using Domain.SeedWork.Interfaces;

namespace Domain.AggregateModels;

public abstract class UniqueEntity : IIdentifiable<Guid>
{
    public Guid Id { get; } = Guid.NewGuid();
    public DateTime CreationDateTime { get; private set;  } = DateTime.UtcNow;

    public override bool Equals(object? obj)
        => obj?.GetType() == GetType() && Id.Equals(((UniqueEntity)obj).Id);

    public override int GetHashCode() => Id.GetHashCode();
}
