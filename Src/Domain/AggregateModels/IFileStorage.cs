using Domain.AggregateModels.AccessAccountAggregate;
using Domain.SeedWork.Enums;
using Domain.SeedWork.Interfaces;

namespace Domain.AggregateModels;

public interface IFileStorage<T>
    where T : IFile
{
    FileStorageTypes StorageType { get; }
    Task<FilePath> SaveAsync(Stream stream, AccessAccount owner);
    FileStream? Read(FilePath filePath);
    void Delete(FilePath filePath);
    FilePath GetFullPath(T file);
}