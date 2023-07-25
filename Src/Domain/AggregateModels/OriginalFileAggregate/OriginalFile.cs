using Domain.AggregateModels.AccessAccountAggregate;

namespace Domain.AggregateModels.OriginalFileAggregate;

public sealed class OriginalFile : UniqueEntity, IOriginalFile
{
    public Metadata Metadata { get; }
    public StorageData StorageData { get; }
    public AccessAccount Owner { get; }

    public OriginalFile(Metadata metadata, StorageData storageDate, AccessAccount owner)
        => (Metadata, StorageData, Owner) = (metadata, storageDate, owner);
    
#pragma warning disable CS8618
    private OriginalFile() { }
#pragma warning restore CS8618
}