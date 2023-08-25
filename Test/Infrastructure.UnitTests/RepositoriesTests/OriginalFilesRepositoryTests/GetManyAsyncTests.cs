namespace Infrastructure.UnitTests.RepositoriesTests.OriginalFilesRepositoryTests;

public class GetManyAsyncTests : BaseRepositoryTests
{
    private OriginalFilesRepository Repository { get; }

    public GetManyAsyncTests() : base("OriginalFilesRepository_GetManyAsyncTests")
        => Repository = new(DbContext);
    
    [Fact]
    private async void GetManyAsync_ReturnsEmptyList_GivenNonExistingOwner()
    {
        var (_, entities) = await Repository.GetManyAsync(
            "non@existing.owner", 
            QueryMediaTypes.All,
            builder => builder.ApplyLimit(3)
                .ApplyOffset(2)
                .ApplyOrder("name:asc"));

        entities.Should()
            .BeEmpty();
    }
    
    [Fact]
    private async void GetManyAsync_ReturnsCorrectEntities_GivenExistingOwner()
    {
        var (_, entities) = await Repository.GetManyAsync(
            AccessAccounts[1], 
            QueryMediaTypes.All,
            builder => builder.ApplyLimit(100)
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
        var (_, entities) = await Repository.GetManyAsync(
            AccessAccounts[1], 
            QueryMediaTypes.All,
            builder => builder.ApplyLimit(limit)
                .ApplyOffset(0)
                .ApplyOrder("name:asc"));

        entities.Should().HaveCount(limit);
    }
    
    [Fact]
    private async void GetManyAsync_SkipsRecordsUpToOffset_GivenOffsetUnderMaximum()
    {
        var (_, entities) = await Repository.GetManyAsync(
            AccessAccounts[1], 
            QueryMediaTypes.All,
            builder => builder.ApplyLimit(100)
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
        var (_, entities) = await Repository.GetManyAsync(
            AccessAccounts[1], 
            QueryMediaTypes.All,
            builder => builder.ApplyLimit(100)
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
        var (_, entities) = await Repository.GetManyAsync(
            AccessAccounts[1], 
            QueryMediaTypes.All,
            builder => builder.ApplyLimit(100)
                .ApplyOffset(0)
                .ApplyOrder("name:asc"));

        entities.Should()
            .BeInAscendingOrder(entity => entity.Metadata.Name);
    }
    
    [Fact]
    private async void GetManyAsync_ReturnsEntitiesSortedByNameDescending_GivenOrderOfNameDesc()
    {
        var (_, entities) = await Repository.GetManyAsync(
            AccessAccounts[1], 
            QueryMediaTypes.All,
            builder => builder.ApplyLimit(100)
                .ApplyOffset(0)
                .ApplyOrder("name:desc"));

        entities.Should()
            .BeInDescendingOrder(entity => entity.Metadata.Name);
    }
    
    [Fact]
    private async void GetManyAsync_ReturnsEntitiesSortedByCreationDateTimeAscending_GivenOrderOfDateAsc()
    {
        var (_, entities) = await Repository.GetManyAsync(
            AccessAccounts[1], 
            QueryMediaTypes.All,
            builder => builder.ApplyLimit(100)
                .ApplyOffset(0)
                .ApplyOrder("date:asc"));

        entities.Should()
            .BeInAscendingOrder(entity => entity.CreationDateTime);
    }
    
    [Fact]
    private async void GetManyAsync_ReturnsEntitiesSortedByCreationDateTimeDescending_GivenOrderOfDateDesc()
    {
        var (_, entities) = await Repository.GetManyAsync(
            AccessAccounts[1], 
            QueryMediaTypes.All,
            builder => builder.ApplyLimit(100)
                .ApplyOffset(0)
                .ApplyOrder("date:desc"));

        entities.Should()
            .BeInDescendingOrder(entity => entity.CreationDateTime);
    }
    
    [Fact]
    private async void GetManyAsync_ReturnsEntitiesWithAnyMetadataType_GivenQueryMediaTypesOfAll()
    {
        var (_, entities) = await Repository.GetManyAsync(
            AccessAccounts[1], 
            QueryMediaTypes.All,
            builder => builder.ApplyLimit(100)
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
        var (_, entities) = await Repository.GetManyAsync(
            AccessAccounts[1], 
            QueryMediaTypes.Images,
            builder => builder.ApplyLimit(100)
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
        var (_, entities) = await Repository.GetManyAsync(
            AccessAccounts[1], 
            QueryMediaTypes.Videos,
            builder => builder.ApplyLimit(100)
                .ApplyOffset(0)
                .ApplyOrder("name:asc"));

        entities.Should()
            .BeEquivalentTo(new[]
            {
                OriginalFiles[4]
            });
    }
    
    [Fact]
    private async void GetManyAsync_ReturnsCountOfAllOwnerVideos_GivenQueryMediaTypesOfVideos()
    {
        var (totalCount, _) = await Repository.GetManyAsync(
            AccessAccounts[1], 
            QueryMediaTypes.Videos,
            builder => builder.ApplyLimit(1)
                .ApplyOffset(100)
                .ApplyOrder("name:asc"));

        totalCount.Should()
            .Be(OriginalFiles.Count(
                file => file.Owner.Equals(AccessAccounts[1]) && file.Metadata.Type.Equals(MediaTypes.Video)));
    }
    
    [Fact]
    private async void GetManyAsync_ReturnsCountOfAllOwnerImages_GivenQueryMediaTypesOfImage()
    {
        var (totalCount, _) = await Repository.GetManyAsync(
            AccessAccounts[1], 
            QueryMediaTypes.Images,
            builder => builder.ApplyLimit(1)
                .ApplyOffset(100)
                .ApplyOrder("name:asc"));

        totalCount.Should()
            .Be(OriginalFiles.Count(
                file => file.Owner.Equals(AccessAccounts[1]) && file.Metadata.Type.Equals(MediaTypes.Image)));
    }
    
    [Fact]
    private async void GetManyAsync_ReturnsCountOfAllOwnerFiles_GivenQueryMediaTypesOfAll()
    {
        var (totalCount, _) = await Repository.GetManyAsync(
            AccessAccounts[1], 
            QueryMediaTypes.All,
            builder => builder.ApplyLimit(1)
                .ApplyOffset(100)
                .ApplyOrder("name:asc"));

        totalCount.Should()
            .Be(OriginalFiles.Count(
                file => file.Owner.Equals(AccessAccounts[1])));
    }
}