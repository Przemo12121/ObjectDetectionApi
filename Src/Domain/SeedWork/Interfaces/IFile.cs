using Domain.AggregateModels;

namespace Domain.SeedWork.Interfaces;

public interface IFile
{
    Metadata Metadata { get; }
    StorageData StorageData { get; }
}