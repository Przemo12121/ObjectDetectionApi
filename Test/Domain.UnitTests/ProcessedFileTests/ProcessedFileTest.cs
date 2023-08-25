namespace Domain.UnitTests.ProcessedFileTests;

public class ProcessedFileTest
{
    protected HashSet<AccessAccount> GetViewers(ProcessedFile obj)
        => (HashSet<AccessAccount>)typeof(ProcessedFile)
            .GetField("_viewers", BindingFlags.Instance | BindingFlags.NonPublic)!
            .GetValue(obj)!;
    
    protected ProcessedFile CreateDummyFile()
        => new(
            AccessAccount.Create("test@email.test"),
            new("filename", MediaTypes.Image),
            new(FileStorageTypes.LocalStorage, "store/path"),
            new("http://url"),
            new AccessAccount[]
            {
                "first@email.test", 
                "second@email.test",
                "third@email.test"
            });
}