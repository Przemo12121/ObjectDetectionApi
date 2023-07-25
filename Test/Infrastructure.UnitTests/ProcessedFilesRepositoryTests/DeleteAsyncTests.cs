using Domain.AggregateModels.AccessAccountAggregate;

namespace Infrastructure.UnitTests.ProcessedFilesRepositoryTests;

public class DeleteAsyncTests : BaseRepositoryTests
{
    private ProcessedFilesRepository Repository { get; init; }
    
    public DeleteAsyncTests() : base("ProcessedFilesRepositoryTests_DeleteAsyncTests")
        => Repository = new(DbContext);
    
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public async void DeleteAsync_ShouldDeleteProcessedFile_GivenExistingEntity(int index)
    {
        await Repository.RemoveAsync(ProcessedFiles[index]);

        DbContext.ProcessedFiles.Should()
            .NotContain(ProcessedFiles[index]);
    }
    
    [Fact]
    public async void DeleteAsync_ShouldNotDeleteAnyEntity_GivenNonExistingEntity()
    {
        ProcessedFile nonExisting = new(
            AccessAccounts[1],
            new("non existing file", MediaTypes.Image),
            new(FileStorages.LocalStorage, "dummy"),
            new("dummy"),
            new List<AccessAccount>() { AccessAccounts[0], AccessAccounts[4] });

        Func<Task> action = async () => await Repository.RemoveAsync(nonExisting);
        await action.Should()
            .ThrowAsync<DbUpdateConcurrencyException>();
        
        DbContext.OriginalFiles.Should()
            .Contain(OriginalFiles);
        DbContext.ProcessedFiles.Should()
            .Contain(ProcessedFiles);
        DbContext.AccessAccounts.Should()
            .Contain(AccessAccounts);
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public async void DeleteAsync_ShouldNotDeleteOtherProcessedFiles_GivenExistingEntity(int index)
    {
        await Repository.RemoveAsync(ProcessedFiles[index]);

        DbContext.ProcessedFiles.Should()
            .Contain(ProcessedFiles.Where(file => !file.Equals(ProcessedFiles[index])));
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public async void DeleteAsync_ShouldNotDeleteOriginalFiles_GivenExistingEntity(int index)
    {
        await Repository.RemoveAsync(ProcessedFiles[index]);

        DbContext.OriginalFiles.Should()
            .Contain(OriginalFiles);
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public async void DeleteAsync_ShouldNotDeleteAccessAccounts_GivenExistingEntity(int index)
    {
        await Repository.RemoveAsync(ProcessedFiles[index]);

        DbContext.AccessAccounts.Should()
            .Contain(AccessAccounts);
    }
}