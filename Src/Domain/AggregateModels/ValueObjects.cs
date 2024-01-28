using Domain.SeedWork.Enums;

namespace Domain.AggregateModels;

public sealed record Metadata(string Name, MediaTypes Type);

public sealed record StorageData(FileStorageTypes StorageType, string Uri);

public sealed record ServeData(string Url);

public sealed record FilePath(string Path)
{
    public override string ToString() => Path;
    public static implicit operator FilePath(string uri) => new(uri);
    public static implicit operator string(FilePath filePath) => filePath.ToString();
}