namespace Infrastructure.UnitTests.FileStorageTests.LocalFileStorageTests;

public class ReadTests : LocalFileStorageTests
{
    public ReadTests() : base("Read") {}

    [Theory]
    [InlineData("TEST_FILE_0")]
    [InlineData("TEST_FILE_1")]
    [InlineData("TEST_FILE_2")]
    public void Read_ReturnsFile_GivenExistingPath(string path)
    {
       var file = LocalFileStorage.Read(path);
       
       file.Should()
           .NotBeNull();
    }
    
    [Theory]
    [InlineData("NOT_EXISTING_FILE_0")]
    [InlineData("NOT_EXISTING_FILE_1")]
    [InlineData("NOT_EXISTING_FILE_2")]
    public void Read_ReturnsNull_GivenNonExistingPath(string path)
    {
        var file = LocalFileStorage.Read(path);

        file.Should()
            .BeNull();
    }
}