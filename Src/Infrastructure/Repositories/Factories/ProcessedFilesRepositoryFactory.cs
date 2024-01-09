using Domain.AggregateModels.ProcessedFileAggregate;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Factories;

public class ProcessedFilesRepositoryFactory : IRepositoryFactory<ProcessedFilesRepository, ProcessedFile>
{
    private readonly DbContextOptions<ObjectDetectionDbContext> _options;

    public ProcessedFilesRepositoryFactory(DbContextOptions<ObjectDetectionDbContext> options)
        => _options = options;

    public ProcessedFilesRepository Create()
        => new(new ObjectDetectionDbContext(_options));
}