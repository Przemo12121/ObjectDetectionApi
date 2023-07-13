namespace Domain.UnitTests.ProcessedFileTests;

public class ProcessedFileTest
{
    protected HashSet<AccessAccount> GetViewers(ProcessedFile obj)
        => (HashSet<AccessAccount>)typeof(ProcessedFile)
            .GetField("<Viewers>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic)!
            .GetValue(obj)!;
    
    protected ProcessedFile CreateDummyFile()
        => new(
            AccessAccount.Create("test@email.test"),
            new("filename", MediaTypes.Image),
            new(FileStorages.LocalStorage, "store/path"),
            new("http://url"),
            new()
            {
                AccessAccount.Create("first@email.test"), 
                AccessAccount.Create("second@email.test"),
                AccessAccount.Create("third@email.test")
            });
}