namespace Infrastructure.UnitTests.RepositoriesTests.OriginalFilesRepositoryTests;

public class GetManyAsyncTests : BaseRepositoryTests
{
    private OriginalFilesRepository Repository { get; }

    public GetManyAsyncTests() : base("OriginalFilesRepository_GetManyAsyncTests")
        => Repository = new(DbContext);
    
    [Fact]
    private async void GetManyAsync_ReturnsEmptyList_GivenNonExistingOwner()
    {
        var entities = await Repository.GetManyAsync(
            "non@existing.owner", 
            builder => builder.ApplySelection(QueryMediaTypes.All)
                .ApplyLimit(3)
                .ApplyOffset(2)
                .ApplyOrder("name:asc"));

        entities.Should()
            .BeEmpty();
    }
    
    [Fact]
    private async void GetManyAsync_ReturnsCorrectEntities_GivenExistingOwner()
    {
        var entities = await Repository.GetManyAsync(
            AccessAccounts[1], 
            builder => builder.ApplySelection(QueryMediaTypes.All)
                .ApplyLimit(100)
                .ApplyOffset(0)
                .ApplyOrder("name:asc"));

        entities.Should()
            .BeEquivalentTo(new []
            {
                OriginalFiles[0],
                OriginalFiles[1],
                OriginalFiles[3],
                OriginalFiles[4]
            });
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(2)]
    [InlineData(4)]
    private async void GetManyAsync_ReturnsUpToGivenLimit_GivenLimitUnderMaximum(int limit)
    {
        var entities = await Repository.GetManyAsync(
            AccessAccounts[1], 
            builder => builder.ApplySelection(QueryMediaTypes.All)
                .ApplyLimit(limit)
                .ApplyOffset(0)
                .ApplyOrder("name:asc"));

        entities.Should().HaveCount(limit);
    }
    
    [Fact]
    private async void GetManyAsync_SkipsRecordsUpToOffset_GivenOffsetUnderMaximum()
    {
        var entities = await Repository.GetManyAsync(
            AccessAccounts[1], 
            builder => builder.ApplySelection(QueryMediaTypes.All)
                .ApplyLimit(100)
                .ApplyOffset(2)
                .ApplyOrder("name:asc"));

        entities.Should()
            .BeEquivalentTo(new []
            {
                OriginalFiles[3],
                OriginalFiles[4]
            });
    }
    
    [Fact]
    private async void GetManyAsync_ReturnsUpMaximumEntities_GivenLimitOverMaximum()
    {
        var entities = await Repository.GetManyAsync(
            AccessAccounts[1], 
            builder => builder.ApplySelection(QueryMediaTypes.All)
                .ApplyLimit(100)
                .ApplyOffset(0)
                .ApplyOrder("name:asc"));

        entities.Should()
            .BeEquivalentTo(new []
            {
                OriginalFiles[0],
                OriginalFiles[1],
                OriginalFiles[3],
                OriginalFiles[4]
            });
    }
    
    [Fact]
    private async void GetManyAsync_ReturnsEntitiesSortedByNameAscending_GivenOrderOfNameAsc()
    {
        var entities = await Repository.GetManyAsync(
            AccessAccounts[1], 
            builder => builder.ApplySelection(QueryMediaTypes.All)
                .ApplyLimit(100)
                .ApplyOffset(0)
                .ApplyOrder("name:asc"));

        entities.Should()
            .BeInAscendingOrder(entity => entity.Metadata.Name);
    }
    
    [Fact]
    private async void GetManyAsync_ReturnsEntitiesSortedByNameDescending_GivenOrderOfNameDesc()
    {
        var entities = await Repository.GetManyAsync(
            AccessAccounts[1], 
            builder => builder.ApplySelection(QueryMediaTypes.All)
                .ApplyLimit(100)
                .ApplyOffset(0)
                .ApplyOrder("name:desc"));

        entities.Should()
            .BeInDescendingOrder(entity => entity.Metadata.Name);
    }
    
    [Fact]
    private async void GetManyAsync_ReturnsEntitiesSortedByCreationDateTimeAscending_GivenOrderOfDateAsc()
    {
        var entities = await Repository.GetManyAsync(
            AccessAccounts[1], 
            builder => builder.ApplySelection(QueryMediaTypes.All)
                .ApplyLimit(100)
                .ApplyOffset(0)
                .ApplyOrder("date:asc"));

        entities.Should()
            .BeInAscendingOrder(entity => entity.CreationDateTime);
    }
    
    [Fact]
    private async void GetManyAsync_ReturnsEntitiesSortedByCreationDateTimeDescending_GivenOrderOfDateDesc()
    {
        var entities = await Repository.GetManyAsync(
            AccessAccounts[1], 
            builder => builder.ApplySelection(QueryMediaTypes.All)
                .ApplyLimit(100)
                .ApplyOffset(0)
                .ApplyOrder("date:desc"));

        entities.Should()
            .BeInDescendingOrder(entity => entity.CreationDateTime);
    }
    
    [Fact]
    private async void GetManyAsync_ReturnsEntitiesWithAnyMetadataType_GivenQueryMediaTypesOfAll()
    {
        var entities = await Repository.GetManyAsync(
            AccessAccounts[1], 
            builder => builder.ApplySelection(QueryMediaTypes.All)
                .ApplyLimit(100)
                .ApplyOffset(0)
                .ApplyOrder("name:asc"));

        entities.Should()
            .BeEquivalentTo(new[]
            {
                OriginalFiles[0],
                OriginalFiles[1],
                OriginalFiles[3],
                OriginalFiles[4]
            });
    }
    
    [Fact]
    private async void GetManyAsync_ReturnsEntitiesWithMetadataTypeOfImage_GivenQueryMediaTypesOfImages()
    {
        var entities = await Repository.GetManyAsync(
            AccessAccounts[1], 
            builder => builder.ApplySelection(QueryMediaTypes.Images)
                .ApplyLimit(100)
                .ApplyOffset(0)
                .ApplyOrder("name:asc"));

        entities.Should()
            .BeEquivalentTo(new[]
            {
                OriginalFiles[0],
                OriginalFiles[1],
                OriginalFiles[3]
            });
    }
    
    [Fact]
    private async void GetManyAsync_ReturnsEntitiesWithMetadataTypeOfVideo_GivenQueryMediaTypesOfVideos()
    {
        var entities = await Repository.GetManyAsync(
            AccessAccounts[1], 
            builder => builder.ApplySelection(QueryMediaTypes.Videos)
                .ApplyLimit(100)
                .ApplyOffset(0)
                .ApplyOrder("name:asc"));

        entities.Should()
            .BeEquivalentTo(new[]
            {
                OriginalFiles[4]
            });
    }
}