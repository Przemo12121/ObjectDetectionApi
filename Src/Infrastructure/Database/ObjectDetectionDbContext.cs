using Domain.AggregateModels.AccessAccountAggregate;
using Domain.AggregateModels.OriginalFileAggregate;
using Domain.AggregateModels.ProcessedFileAggregate;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database;

public sealed class ObjectDetectionDbContext : DbContext
{
    public DbSet<AccessAccount> AccessAccounts { get; set; } = null!;
    public DbSet<OriginalFile> OriginalFiles { get; set; } = null!;
    public DbSet<ProcessedFile> ProcessedFiles { get; set; } = null!;

    public ObjectDetectionDbContext(DbContextOptions<ObjectDetectionDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ConfigureProcessedFile();
        modelBuilder.ConfigureAccessAccount();
        modelBuilder.ConfigureOriginalFile();
    }
}