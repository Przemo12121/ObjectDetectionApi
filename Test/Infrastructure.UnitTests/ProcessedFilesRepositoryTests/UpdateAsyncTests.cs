namespace Infrastructure.UnitTests.ProcessedFilesRepositoryTests;

public class UpdateAsyncTests : BaseRepositoryTests
{
    private ProcessedFilesRepository Repository { get; }

    public UpdateAsyncTests() : base("ProcessedFilesRepository_UpdateAsyncTests")
        => Repository = new(DbContext);

    [Fact]
    public async void UpdateAsync_ShouldNotCreateNewProcessedFile_GivenExistingEntity()
    {
        var entity = ProcessedFiles[2];
        entity.Remove(new[] { AccessAccounts[1], AccessAccounts[9] });
        entity.Add(new[] { AccessAccounts[3], AccessAccounts[7] });

        await Repository.UpdateAsync(entity);
        
        DbContext.ProcessedFiles.Should()
            .Contain(entity)
            .And.HaveCount(ProcessedFiles.Count);
    }
    
    [Fact]
    public async void UpdateAsync_ShouldNotCreateAccessAccounts_GivenExistingViewers()
    {
        var entity = ProcessedFiles[2];
        entity.Remove(new[] { AccessAccounts[1], AccessAccounts[9] });
        entity.Add(new[] { AccessAccounts[3], AccessAccounts[7] });

        await Repository.UpdateAsync(entity);
        
        DbContext.AccessAccounts.Should()
            .Contain(AccessAccounts)
            .And.HaveCount(AccessAccounts.Count);
    }
    
    [Fact]
    public async void UpdateAsync_ShouldCreateAccessAccounts_GivenNonExistingViewers()
    {
        var entity = ProcessedFiles[2];
        entity.Remove(new[] { AccessAccounts[1], AccessAccounts[9] });
        entity.Add(new[] { AccessAccounts[3], "non@existing.viewer", AccessAccounts[7], "non@existing.viewer2" });

        await Repository.UpdateAsync(entity);
        
        DbContext.AccessAccounts.Should()
            .Contain("non@existing.viewer")
            .And.Contain("non@existing.viewer2")
            .And.HaveCount(AccessAccounts.Count + 2);
    }
    
    [Fact]
    public async void UpdateAsync_ShouldUpdateEntityViewers_GivenChangesToViewers()
    {
        var entity = ProcessedFiles[2];
        entity.Remove(new[] { AccessAccounts[1], AccessAccounts[1], AccessAccounts[9], AccessAccounts[4] });
        entity.Add(new[]
        {
            AccessAccounts[3], 
            AccessAccounts[3], 
            "non@existing.viewer", 
            "non@existing.viewer",
            AccessAccounts[7], 
            "non@existing.viewer2"
        });

        await Repository.UpdateAsync(entity);
        
        var entityAfter = DbContext.ProcessedFiles
            .Include(file => file.Viewers)
            .FirstOrDefault(file => file.Equals(entity))!;

        entityAfter.Viewers.Should()
            .BeEquivalentTo(new[]
            {
                AccessAccounts[2],
                AccessAccounts[3],
                AccessAccounts[7],
                "non@existing.viewer",
                "non@existing.viewer2"
            });
    }
    
    [Fact]
    public async void UpdateAsync_ShouldNotUpdateEntityOwner_GivenChangesToViewers()
    {
        var entity = ProcessedFiles[2];
        entity.Remove(new[] { AccessAccounts[1], AccessAccounts[1], AccessAccounts[9], AccessAccounts[4] });
        entity.Add(new[]
        {
            AccessAccounts[3], 
            AccessAccounts[3], 
            "non@existing.viewer", 
            "non@existing.viewer",
            AccessAccounts[7], 
            "non@existing.viewer2"
        });

        await Repository.UpdateAsync(entity);
        
        var entityAfter = DbContext.ProcessedFiles
            .FirstOrDefault(file => file.Equals(entity))!;

        entityAfter.Owner.Should()
            .BeEquivalentTo(ProcessedFiles[2].Owner);
    }
    
    [Fact]
    public async void UpdateAsync_ShouldNotUpdateEntityData_GivenChangesToViewers()
    {
        var entity = ProcessedFiles[2];
        entity.Remove(new[] { AccessAccounts[1], AccessAccounts[1], AccessAccounts[9], AccessAccounts[4] });
        entity.Add(new[]
        {
            AccessAccounts[3], 
            AccessAccounts[3], 
            "non@existing.viewer", 
            "non@existing.viewer",
            AccessAccounts[7], 
            "non@existing.viewer2"
        });

        await Repository.UpdateAsync(entity);
        
        var entityAfter = DbContext.ProcessedFiles
            .FirstOrDefault(file => file.Equals(entity))!;

        entityAfter.Id.Should()
            .Be(ProcessedFiles[2].Id);
        entityAfter.CreationDateTime.Should()
            .Be(ProcessedFiles[2].CreationDateTime);
        entityAfter.Metadata.Should()
            .BeEquivalentTo(ProcessedFiles[2].Metadata);
        entityAfter.StorageData.Should()
            .BeEquivalentTo(ProcessedFiles[2].StorageData);
        entityAfter.ServeData.Should()
            .BeEquivalentTo(ProcessedFiles[2].ServeData);
    }
    
    [Fact]
    public async void UpdateAsync_ShouldNotUpdateOtherEntitiesViewers_GivenChangesToViewers()
    {
        var entity = ProcessedFiles[2];
        entity.Remove(new[] { AccessAccounts[1], AccessAccounts[1], AccessAccounts[9], AccessAccounts[4] });
        entity.Add(new[]
        {
            AccessAccounts[3], 
            AccessAccounts[3], 
            "non@existing.viewer", 
            "non@existing.viewer",
            AccessAccounts[7], 
            "non@existing.viewer2"
        });

        await Repository.UpdateAsync(entity);
        
        var entityAfter = DbContext.ProcessedFiles
            .Include(file => file.Viewers)
            .FirstOrDefault(file => file.Equals(ProcessedFiles[1]))!;

        entityAfter.Viewers.Should()
            .BeEquivalentTo(ProcessedFiles[1].Viewers);
    }
}