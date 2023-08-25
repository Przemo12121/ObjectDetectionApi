namespace Domain.UnitTests.ProcessedFileTests;

public class GetViewersTests
{
    [Fact]
    public void GetViewers_ReturnsCorrectlyFormattedList_GivenMisFormatted()
    {
        ProcessedFile tested = new(
            AccessAccount.Create("test@email.test"),
            new("Test/path", MediaTypes.Image),
            new(FileStorageTypes.LocalStorage, "store/path"),
            new("http://url"),
            new[]
            {
                AccessAccount.Create("firSt@eMail.TESt"), 
                AccessAccount.Create("second@email.test"),
                AccessAccount.Create("sECond@EMail.test"),
                AccessAccount.Create("THIRD@email.Test")
            });

        var emails = tested.Viewers.Select(viewer => viewer.Id);

        emails.Should().BeEquivalentTo(new[] { "first@email.test", "second@email.test", "third@email.test" });
    }
}