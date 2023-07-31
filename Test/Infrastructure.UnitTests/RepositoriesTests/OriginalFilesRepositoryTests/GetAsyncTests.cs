namespace Infrastructure.UnitTests.RepositoriesTests.OriginalFilesRepositoryTests;

public class GetAsyncTests : BaseRepositoryTests
{
    private OriginalFilesRepository Repository { get; }

    public GetAsyncTests() : base("OriginalFilesRepository_GetAsyncTests")
        => Repository = new(DbContext);
    
    [Fact]
    private async void GetAsync_ReturnsEntity_GivenExistingId()
    {
        var entity = await Repository.GetAsync(OriginalFiles[1].Id);

        entity.Should()
            .NotBeNull();
    }
    
    [Fact]
    private async void GetAsync_ReturnsAllEntityData_GivenExistingId()
    {
        var entity = await Repository.GetAsync(OriginalFiles[1].Id);

        entity.Should()
            .BeEquivalentTo(OriginalFiles[1]);
    }
    
    [Fact]
    private async void GetAsync_NotReturnsEntity_GivenNonExistingId()
    {
        var entity = await Repository.GetAsync(Guid.NewGuid());

        entity.Should()
            .BeNull();
    }
    
}