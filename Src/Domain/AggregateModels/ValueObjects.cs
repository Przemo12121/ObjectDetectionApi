using Domain.SeedWork.Enums;

namespace Domain.AggregateModels;

public sealed record Metadata(string Name, MediaTypes Type);

public sealed record StorageData(FileStorages Storage, string Uri);

public sealed record ServeData(string Url);
