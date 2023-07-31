namespace Infrastructure.UnitTests.RepositoriesTests.ProcessedFilesRepositoryTests;

public class AddAsyncTests : BaseRepositoryTests
{
    private ProcessedFilesRepository Repository { get; }

    public AddAsyncTests() : base("ProcessedFilesRepository_AddAsyncTests")
        => Repository = new(DbContext);

    [Fact]
    public async void AddAsync_CreatesNewProcessedFile_GivenNewEntity()
    {
        ProcessedFile newEntity = new(
            AccessAccounts[3],
            new("new_file", MediaTypes.Image),
            new(FileStorages.LocalStorage, "new_file_uri"),
            new("new_url"),
            new List<AccessAccount>
            {
                AccessAccounts[1],
                AccessAccounts[1],
                AccessAccounts[4],
                "new.viewer@access.account"
            });

        await Repository.AddAsync(newEntity);

        DbContext.ProcessedFiles.Should()
            .Contain(newEntity)
            .And.HaveCount(ProcessedFiles.Count + 1);
    }
    
    [Fact]
    public async void AddAsync_NotCreatesNewAccessAccounts_GivenExistingOwnerAndViewers()
    {
        ProcessedFile newEntity = new(
            AccessAccounts[3],
            new("new_file", MediaTypes.Image),
            new(FileStorages.LocalStorage, "new_file_uri"),
            new("new_url"),
            new List<AccessAccount>
            {
                AccessAccounts[1],
                AccessAccounts[1],
                AccessAccounts[4]
            });

        await Repository.AddAsync(newEntity);

        DbContext.AccessAccounts.Should()
            .Contain(AccessAccounts[3])
            .And.HaveCount(AccessAccounts.Count);
    }
    
    [Fact]
    public async void AddAsync_CreatesNewAccessAccount_GivenNonExistingOwner()
    {
        ProcessedFile newEntity = new(
            "new.owner@access.account",
            new("new_file", MediaTypes.Image),
            new(FileStorages.LocalStorage, "new_file_uri"),
            new("new_url"),
            new List<AccessAccount>
            {
                AccessAccounts[1],
                AccessAccounts[1],
                AccessAccounts[4]
            });

        await Repository.AddAsync(newEntity);

        DbContext.AccessAccounts.Should()
            .Contain("new.owner@access.account")
            .And.HaveCount(AccessAccounts.Count + 1);
    }
    
    [Fact]
    public async void AddAsync_CreatesOnlyNewAccessAccounts_GivenNonExistingViewers()
    {
        ProcessedFile newEntity = new(
            AccessAccounts[3],
            new("new_file", MediaTypes.Image),
            new(FileStorages.LocalStorage, "new_file_uri"),
            new("new_url"),
            new List<AccessAccount>
            {
                AccessAccounts[1],
                AccessAccounts[1],
                AccessAccounts[4],
                "new.viewer@access.account"
            });

        await Repository.AddAsync(newEntity);

        DbContext.AccessAccounts.Should()
            .Contain("new.viewer@access.account")
            .And.HaveCount(AccessAccounts.Count + 1);
    }
}