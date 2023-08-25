namespace Infrastructure.UnitTests.RepositoriesTests.ProcessedFilesRepositoryTests;

public class DeleteAsyncTests : BaseRepositoryTests
{
    private ProcessedFilesRepository Repository { get; init; }
    
    public DeleteAsyncTests() : base("ProcessedFilesRepositoryTests_DeleteAsyncTests")
        => Repository = new(DbContext);
    
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public async void DeleteAsync_DeletesProcessedFile_GivenExistingEntity(int index)
    {
        await Repository.RemoveAsync(ProcessedFiles[index]);

        DbContext.ProcessedFiles.Should()
            .NotContain(ProcessedFiles[index]);
    }
    
    [Fact]
    public async void DeleteAsync_NotDeletesAnyEntity_GivenNonExistingEntity()
    {
        ProcessedFile nonExisting = new(
            AccessAccounts[1],
            new("non existing file", MediaTypes.Image),
            new(FileStorageTypes.LocalStorage, "dummy"),
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
    public async void DeleteAsync_NotDeletesOtherProcessedFiles_GivenExistingEntity(int index)
    {
        await Repository.RemoveAsync(ProcessedFiles[index]);

        DbContext.ProcessedFiles.Should()
            .Contain(ProcessedFiles.Where(file => !file.Equals(ProcessedFiles[index])));
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public async void DeleteAsync_NotDeletesOriginalFiles_GivenExistingEntity(int index)
    {
        await Repository.RemoveAsync(ProcessedFiles[index]);

        DbContext.OriginalFiles.Should()
            .Contain(OriginalFiles);
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public async void DeleteAsync_NotDeletesAccessAccounts_GivenExistingEntity(int index)
    {
        await Repository.RemoveAsync(ProcessedFiles[index]);

        DbContext.AccessAccounts.Should()
            .Contain(AccessAccounts);
    }
}