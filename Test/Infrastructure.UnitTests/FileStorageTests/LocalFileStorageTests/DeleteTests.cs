namespace Infrastructure.UnitTests.FileStorageTests.LocalFileStorageTests;

public class DeleteTests : LocalFileStorageTests
{
    public DeleteTests() : base("Delete") {}

    [Theory]
    [InlineData("TEST_FILE_0")]
    [InlineData("TEST_FILE_1")]
    [InlineData("TEST_FILE_2")]
    public void Delete_DeletesFile_GivenExistingPath(string path)
    {
        LocalFileStorage.Delete(path);
        
        File.Exists($"{TargetDirectory}/{path}")
            .Should()
            .BeFalse();
    }
    
    [Theory]
    [InlineData("TEST_FILE_0")]
    [InlineData("TEST_FILE_1")]
    [InlineData("TEST_FILE_2")]
    public void Delete_NotDeletesOtherFiles_GivenExistingPath(string path)
    {
        LocalFileStorage.Delete(path);
        
        ExistingFiles.Where(file => file != path)
            .ToList()
            .ForEach(
                file => File.Exists($"{TargetDirectory}/{file}")
                    .Should()
                    .BeTrue());
    }
    
    [Theory]
    [InlineData("NOT_EXISTING_FILE_0")]
    [InlineData("NOT_EXISTING_FILE_1")]
    [InlineData("NOT_EXISTING_FILE_2")]
    public void Delete_NotDeletesAnyFile_GivenNonExistingPath(string path)
    {
        LocalFileStorage.Delete(path);
        
        ExistingFiles.ForEach(
            file => File.Exists($"{TargetDirectory}/{file}")
                .Should()
                .BeTrue());
    }
}