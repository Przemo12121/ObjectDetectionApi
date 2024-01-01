using Domain.SeedWork.Interfaces;
using Newtonsoft.Json;

namespace Domain.AggregateModels;

public abstract class UniqueEntity : IIdentifiable<Guid>
{
    // private setters and attributes required by json deserialization in amqp communication
    
    [JsonProperty]
    public Guid Id { get; private set; } = Guid.NewGuid();
    
    [JsonProperty]
    public DateTime CreationDateTime { get; private set; } = DateTime.UtcNow;

    public override bool Equals(object? obj)
        => obj?.GetType() == GetType() && Id.Equals(((UniqueEntity)obj).Id);

    public override int GetHashCode() => Id.GetHashCode();
}
