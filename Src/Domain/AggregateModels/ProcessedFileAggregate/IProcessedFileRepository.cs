namespace Domain.AggregateModels.ProcessedFileAggregate;

public interface IProcessedFileRepository : IUniqueEntityRepository<ProcessedFile>
{
    Task UpdateAsync(ProcessedFile entity);
}