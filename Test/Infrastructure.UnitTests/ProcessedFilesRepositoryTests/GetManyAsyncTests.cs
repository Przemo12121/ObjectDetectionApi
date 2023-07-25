namespace Infrastructure.UnitTests.ProcessedFilesRepositoryTests;

public class GetManyAsyncTests : BaseRepositoryTests
{
    private ProcessedFilesRepository Repository { get; }

    public GetManyAsyncTests() : base("ProcessedFilesRepository_GetManyAsyncTests")
        => Repository = new(DbContext);
    
    [Fact]
    private async void GetManyAsync_ShouldReturnEmptyList_GivenNonExistingOwner()
    {
        var entities = await Repository.GetManyAsync(
            "non@existing.owner", 
            builder => builder.ApplyLimit(3)
                .ApplyOffset(2)
                .ApplyOrder("name:asc"));

        entities.Should()
            .BeEmpty();
    }
    
    [Fact]
    private async void GetManyAsync_ShouldReturnCorrectEntities_GivenExistingOwner()
    {
        var entities = await Repository.GetManyAsync(
            AccessAccounts[3], 
            builder => builder.ApplyLimit(100)
                .ApplyOffset(0)
                .ApplyOrder("name:asc"));

        entities.Should()
            .BeEquivalentTo(new []
            {
                ProcessedFiles[1],
                ProcessedFiles[2],
                ProcessedFiles[3],
                ProcessedFiles[4]
            });
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(2)]
    [InlineData(4)]
    private async void GetManyAsync_ShouldReturnUpToGivenLimit_GivenLimitUnderMaximum(int limit)
    {
        var entities = await Repository.GetManyAsync(
            AccessAccounts[3], 
            builder => builder.ApplyLimit(limit)
                .ApplyOffset(0)
                .ApplyOrder("name:asc"));

        entities.Should().HaveCount(limit);
    }
    
    [Fact]
    private async void GetManyAsync_ShouldSkipRecordsUpToOffset_GivenOffsetUnderMaximum()
    {
        var entities = await Repository.GetManyAsync(
            AccessAccounts[3], 
            builder => builder.ApplyLimit(100)
                .ApplyOffset(2)
                .ApplyOrder("name:asc"));

        entities.Should()
            .BeEquivalentTo(new []
            {
                ProcessedFiles[3],
                ProcessedFiles[4]
            });
    }
    
    [Fact]
    private async void GetManyAsync_ShouldReturnUpMaximumEntities_GivenLimitOverMaximum()
    {
        var entities = await Repository.GetManyAsync(
            AccessAccounts[3], 
            builder => builder.ApplyLimit(100)
                .ApplyOffset(0)
                .ApplyOrder("name:asc"));

        entities.Should()
            .BeEquivalentTo(new []
            {
                ProcessedFiles[1],
                ProcessedFiles[2],
                ProcessedFiles[3],
                ProcessedFiles[4]
            });
    }
    
    [Fact]
    private async void GetManyAsync_ShouldReturnEntitiesSortedByNameAscending_GivenOrderOfNameAsc()
    {
        var entities = await Repository.GetManyAsync(
            AccessAccounts[3], 
            builder => builder.ApplyLimit(100)
                .ApplyOffset(0)
                .ApplyOrder("name:asc"));

        entities.Should()
            .BeInAscendingOrder(entity => entity.Metadata.Name);
    }
    
    [Fact]
    private async void GetManyAsync_ShouldReturnEntitiesSortedByNameDescending_GivenOrderOfNameDesc()
    {
        var entities = await Repository.GetManyAsync(
            AccessAccounts[3], 
            builder => builder.ApplyLimit(100)
                .ApplyOffset(0)
                .ApplyOrder("name:desc"));

        entities.Should()
            .BeInDescendingOrder(entity => entity.Metadata.Name);
    }
    
    [Fact]
    private async void GetManyAsync_ShouldReturnEntitiesSortedByCreationDateTimeAscending_GivenOrderOfDateAsc()
    {
        var entities = await Repository.GetManyAsync(
            AccessAccounts[3], 
            builder => builder.ApplyLimit(100)
                .ApplyOffset(0)
                .ApplyOrder("date:asc"));

        entities.Should()
            .BeInAscendingOrder(entity => entity.CreationDateTime);
    }
    
    [Fact]
    private async void GetManyAsync_ShouldReturnEntitiesSortedByCreationDateTimeDescending_GivenOrderOfDateDesc()
    {
        var entities = await Repository.GetManyAsync(
            AccessAccounts[3], 
            builder => builder.ApplyLimit(100)
                .ApplyOffset(0)
                .ApplyOrder("date:desc"));

        entities.Should()
            .BeInDescendingOrder(entity => entity.CreationDateTime);
    }
}