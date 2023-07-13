using Domain.SeedWork.Enums;

namespace Domain.AggregateModels;

public record Metadata(string Name, MediaTypes Type);

public record StorageData(FileStorages Storage, string Uri);

public record ServeData(string Url);
