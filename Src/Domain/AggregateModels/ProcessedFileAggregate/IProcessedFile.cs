using Domain.AggregateModels.AccessAccountAggregate;
using Domain.SeedWork.Interfaces;

namespace Domain.AggregateModels.ProcessedFileAggregate;

public interface IProcessedFile : IIdentifiable<Guid>, IFile, IOwnable<AccessAccount>, IServable
{
    
}