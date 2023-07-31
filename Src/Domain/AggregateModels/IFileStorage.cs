using Domain.AggregateModels.AccessAccountAggregate;
using Domain.SeedWork.Interfaces;

namespace Domain.AggregateModels;

public interface IFileStorage<T>
    where T : IFile
{
    Task<FilePath> SaveAsync(Stream stream, AccessAccount owner);
    FileStream? Read(FilePath filePath);
    void Delete(FilePath filePath);
}