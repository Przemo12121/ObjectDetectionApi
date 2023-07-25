namespace Infrastructure.UnitTests.OriginalFilesRepositoryTests;

public class DeleteAsyncTests : BaseRepositoryTests
{
    private OriginalFilesRepository Repository { get; init; }
    
    public DeleteAsyncTests() : base("OriginalFilesRepositoryTests_DeleteAsyncTests")
        => Repository = new(DbContext);
    
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public async void DeleteAsync_ShouldDeleteOriginalFile_GivenExistingEntity(int index)
    {
        await Repository.RemoveAsync(OriginalFiles[index]);

        DbContext.OriginalFiles.Should()
            .NotContain(OriginalFiles[index]);
    }
    
    [Fact]
    public async void DeleteAsync_ShouldNotDeleteAnyEntity_GivenNonExistingEntity()
    {
        OriginalFile nonExisting = new(
            new("non existing file", MediaTypes.Image),
            new(FileStorages.LocalStorage, "dummy"),
            AccessAccounts[1]);

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
    public async void DeleteAsync_ShouldNotDeleteOtherOriginalFiles_GivenExistingEntity(int index)
    {
        await Repository.RemoveAsync(OriginalFiles[index]);

        DbContext.OriginalFiles.Should()
            .Contain(OriginalFiles.Where(file => !file.Equals(OriginalFiles[index])));
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public async void DeleteAsync_ShouldNotDeleteProcessedFiles_GivenExistingEntity(int index)
    {
        await Repository.RemoveAsync(OriginalFiles[index]);

        DbContext.ProcessedFiles.Should()
            .Contain(ProcessedFiles);
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public async void DeleteAsync_ShouldNotDeleteAccessAccounts_GivenExistingEntity(int index)
    {
        await Repository.RemoveAsync(OriginalFiles[index]);

        DbContext.AccessAccounts.Should()
            .Contain(AccessAccounts);
    }
}