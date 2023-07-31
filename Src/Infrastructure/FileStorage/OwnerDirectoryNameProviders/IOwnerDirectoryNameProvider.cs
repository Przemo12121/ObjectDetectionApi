using Domain.AggregateModels.AccessAccountAggregate;

namespace Infrastructure.FileStorage.OwnerDirectoryNameProviders;

public interface IOwnerDirectoryNameProvider
{
    public string GetFor(AccessAccount owner);
}