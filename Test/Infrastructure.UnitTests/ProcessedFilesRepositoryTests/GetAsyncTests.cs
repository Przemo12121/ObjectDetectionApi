namespace Infrastructure.UnitTests.ProcessedFilesRepositoryTests;

public class GetAsyncTests : BaseRepositoryTests
{
    private ProcessedFilesRepository Repository { get; }

    public GetAsyncTests() : base("ProcessedFilesRepository_GetAsyncTests")
        => Repository = new(DbContext);
    
    [Fact]
    private async void GetAsync_ShouldReturnEntity_GivenExistingId()
    {
        var entity = await Repository.GetAsync(ProcessedFiles[1].Id);

        entity.Should()
            .NotBeNull();
    }
    
    [Fact]
    private async void GetAsync_ShouldReturnAllEntityData_GivenExistingId()
    {
        var entity = await Repository.GetAsync(ProcessedFiles[1].Id);

        entity.Should()
            .BeEquivalentTo(ProcessedFiles[1]);
    }
    
    [Fact]
    private async void GetAsync_ShouldNotReturnEntity_GivenNonExistingId()
    {
        var entity = await Repository.GetAsync(Guid.NewGuid());

        entity.Should()
            .BeNull();
    }
    
}