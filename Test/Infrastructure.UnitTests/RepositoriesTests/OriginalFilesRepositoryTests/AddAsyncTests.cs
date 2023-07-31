namespace Infrastructure.UnitTests.RepositoriesTests.OriginalFilesRepositoryTests;

public class AddAsyncTests : BaseRepositoryTests
{
    private OriginalFilesRepository Repository { get; }

    public AddAsyncTests() : base("OriginalFilesRepository_AddAsyncTests")
        => Repository = new(DbContext);

    [Fact]
    public async void AddAsync_CreatesNewOriginalFile_GivenNewEntity()
    {
        OriginalFile newEntity = new(
            new("new_file", MediaTypes.Image),
            new(FileStorages.LocalStorage, "new_file_uri"),
            AccessAccounts[3]);

        await Repository.AddAsync(newEntity);

        DbContext.OriginalFiles.Should()
            .Contain(newEntity)
            .And.HaveCount(OriginalFiles.Count + 1);
    }
    
    [Fact]
    public async void AddAsync_NotCreatesNewAccessAccount_GivenExistingOwner()
    {
        OriginalFile newEntity = new(
            new("new_file", MediaTypes.Image),
            new(FileStorages.LocalStorage, "new_file_uri"),
            AccessAccounts[3]);

        await Repository.AddAsync(newEntity);

        DbContext.AccessAccounts.Should()
            .Contain(AccessAccounts[3])
            .And.HaveCount(AccessAccounts.Count);
    }
    
    [Fact]
    public async void AddAsync_CreatesNewAccessAccount_GivenNonExistingOwner()
    {
        OriginalFile newEntity = new(
            new("new_file", MediaTypes.Image),
            new(FileStorages.LocalStorage, "new_file_uri"),
            "new@access.account");

        await Repository.AddAsync(newEntity);

        DbContext.AccessAccounts.Should()
            .Contain("new@access.account")
            .And.HaveCount(AccessAccounts.Count + 1);
    }
}