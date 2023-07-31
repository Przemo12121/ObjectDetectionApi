namespace Domain.AggregateModels.ProcessedFileAggregate;

public interface IProcessedFileRepository : IFileRepository<ProcessedFile>
{
    Task UpdateAsync(ProcessedFile entity);
}