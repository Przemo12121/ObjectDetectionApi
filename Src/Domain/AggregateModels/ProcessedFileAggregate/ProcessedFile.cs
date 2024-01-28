using Domain.AggregateModels.AccessAccountAggregate;

namespace Domain.AggregateModels.ProcessedFileAggregate;

public sealed class ProcessedFile : UniqueEntity, IProcessedFile
{
    private readonly HashSet<AccessAccount> _viewers;

    public StorageData StorageData { get; }
    public ServeData ServeData { get; set; }
    public Metadata Metadata { get; }
    public AccessAccount Owner { get; }

    public IReadOnlySet<AccessAccount> Viewers
    {
        get => _viewers;
        private init => _viewers = new HashSet<AccessAccount>(value);  // overrides efcore's LegacyReferenceComparer
    }

    public ProcessedFile(
        AccessAccount owner,
        Metadata metadata,
        StorageData storageData,
        ServeData serveData,
        ICollection<AccessAccount> viewers
    )
        => (Owner, Metadata, StorageData, ServeData, _viewers) = (owner, metadata, storageData, serveData, new(viewers));

#pragma warning disable CS8618
    private ProcessedFile() { }
#pragma warning restore CS8618

    public void Add(AccessAccount viewer)
        => _viewers.Add(viewer);

    public void Add(ICollection<AccessAccount> viewers)
        => _viewers.UnionWith(viewers);

    public void Remove(AccessAccount viewer)
        => _viewers.Remove(viewer);

    public void Remove(ICollection<AccessAccount> viewers)
        => _viewers.RemoveWhere(viewers.Contains);
}