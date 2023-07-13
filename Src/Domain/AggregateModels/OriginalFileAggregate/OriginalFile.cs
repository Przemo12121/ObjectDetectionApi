using Domain.AggregateModels.AccessAccountAggregate;

namespace Domain.AggregateModels.OriginalFileAggregate;

public class OriginalFile : UniqueEntity, IOriginalFile
{
    public Metadata Metadata { get; }
    public StorageData StorageData { get; }
    public AccessAccount Owner { get; }

    public OriginalFile(Metadata metadata, StorageData storageDate, AccessAccount owner)
        => (Metadata, StorageData, Owner) = (metadata, storageDate, owner);
}