using Domain.AggregateModels;
using Domain.AggregateModels.AccessAccountAggregate;
using Domain.SeedWork.Enums;
using Domain.SeedWork.Interfaces;
using Infrastructure.FileStorage.OwnerDirectoryNameProviders;

namespace Infrastructure.FileStorage;

public sealed class LocalFileStorage<T> : IFileStorage<T>, IStorage
    where T : IFile
{
    private readonly string _directoryGlobalPath;
    private readonly IOwnerDirectoryNameProvider _ownerDirectoryNameNameProvider;

    public FileStorageTypes StorageType { get; } = FileStorageTypes.LocalStorage;
    
    public LocalFileStorage(string directoryGlobalPath, IOwnerDirectoryNameProvider ownerDirectoryNameProvider)
        => (_directoryGlobalPath, _ownerDirectoryNameNameProvider) = (directoryGlobalPath, ownerDirectoryNameProvider);

    public async Task<FilePath> SaveAsync(Stream stream, AccessAccount owner)
    {
        var ownerDirectoryName = _ownerDirectoryNameNameProvider.GetFor(owner);
        Directory.CreateDirectory($"{_directoryGlobalPath}/{ownerDirectoryName}");
        
        FilePath path = $"{ownerDirectoryName}/{Guid.NewGuid():N}";
        var file = File.Create($"{_directoryGlobalPath}/{path}");
        
        await stream.CopyToAsync(file);
        await file.FlushAsync();
        file.Close();
        
        return path;
    }

    public FileStream? Read(FilePath filePath)
    {
        var path = $"{_directoryGlobalPath}/{filePath}";

        return File.Exists(path)
            ? File.Open(path, FileMode.Open)
            : null;
    }    
    
    public void Delete(FilePath filePath)
        => File.Delete($"{_directoryGlobalPath}/{filePath}");

    public void EnsureCreated()
        => Directory.CreateDirectory(_directoryGlobalPath);
}