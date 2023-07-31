namespace Infrastructure.UnitTests.FileStorageTests.LocalFileStorageTests;

public abstract class LocalFileStorageTests : IDisposable
{
    protected string TargetDirectory { get; }
    protected LocalFileStorage<OriginalFile> LocalFileStorage { get; }
    protected List<string> ExistingFiles { get; } = new() { "TEST_FILE_0", "TEST_FILE_1", "TEST_FILE_2" };


    protected LocalFileStorageTests(string subPath)
    {
        TargetDirectory = $"./TestFiles/{subPath}";
        Directory.CreateDirectory(TargetDirectory);
        
        var directoryNameProviderMock = new Mock<IOwnerDirectoryNameProvider>();
        directoryNameProviderMock.Setup(
                provider => provider.GetFor(It.IsAny<AccessAccount>()))
            .Returns(String.Empty);

        LocalFileStorage = new(TargetDirectory, directoryNameProviderMock.Object);
        
        ExistingFiles.ForEach(path =>
        {
            var file = File.Create($"{TargetDirectory}/{path}");
            file.Flush();
            file.Close();
        });
    }

    public void Dispose()
        => Directory.Delete(TargetDirectory, true);
}