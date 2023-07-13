using Domain.AggregateModels.AccessAccountAggregate;
using Domain.SeedWork.Interfaces;

namespace Domain.AggregateModels.OriginalFileAggregate;

public interface IOriginalFile : IIdentifiable<Guid>, IFile, IOwnable<AccessAccount>
{
    
}