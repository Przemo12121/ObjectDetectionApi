using Domain.AggregateModels.AccessAccountAggregate;

namespace Domain.AggregateModels.ProcessedFileAggregate;

public class ProcessedFile : UniqueEntity, IProcessedFile
{
    public StorageData StorageData { get; }
    public ServeData ServeData { get; }
    public Metadata Metadata { get; }
    public AccessAccount Owner { get; }
    private HashSet<AccessAccount> Viewers { get; }

    public ProcessedFile(
        AccessAccount owner,
        Metadata metadata,
        StorageData storageData,
        ServeData serveData,
        HashSet<AccessAccount> viewers
    )
        => (Owner, Metadata, StorageData, ServeData, Viewers) = (owner, metadata, storageData, serveData, viewers);

    public void Add(AccessAccount viewer) 
        => Viewers.Add(viewer);

    public void Add(ICollection<AccessAccount> viewers) 
        => Viewers.UnionWith(viewers);

    public void Remove(AccessAccount viewer) 
        => Viewers.Remove(viewer);
    
    public void Remove(ICollection<AccessAccount> viewers) 
        => Viewers.RemoveWhere(viewer => viewers.Contains(viewer));
    
    public IReadOnlyList<AccessAccount> GetViewers() 
        => Viewers
            .ToList()
            .AsReadOnly();    
}