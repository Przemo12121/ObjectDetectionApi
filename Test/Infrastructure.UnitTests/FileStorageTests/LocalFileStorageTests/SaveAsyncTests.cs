namespace Infrastructure.UnitTests.FileStorageTests.LocalFileStorageTests;

public class SaveAsyncTests : LocalFileStorageTests
{
    public SaveAsyncTests() : base("SaveAsync") {}

    [Fact]
    public async void SaveAsync_CreatesNewFileInDirectory_GivenStream()
    {
        var before = Directory.GetFiles(TargetDirectory);
        
        await LocalFileStorage.SaveAsync(new MemoryStream(), "any@owner.test");
        
        var after = Directory.GetFiles(TargetDirectory);
        after.Length.Should()
            .Be(before.Length + 1);
    }
    
    [Fact]
    public async void SaveAsync_CreatesNewFileInDirectoryEachTime_GivenExactSameStream()
    {
        var before = Directory.GetFiles(TargetDirectory);
        
        await LocalFileStorage.SaveAsync(new MemoryStream(), "any@owner.test");
        await LocalFileStorage.SaveAsync(new MemoryStream(), "any@owner.test");
        
        var after = Directory.GetFiles(TargetDirectory);
        after.Length.Should()
            .Be(before.Length + 2);
    }
}