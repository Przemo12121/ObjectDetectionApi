namespace Domain.UnitTests.ProcessedFileTests;

public class RemoveTests : ProcessedFileTest
{
    [Fact]
    public void Remove_RemovesFromViewers_SingleViewer()
    {
        var tested = CreateDummyFile();
        
        tested.Remove(AccessAccount.Create("first@email.test"));

        GetViewers(tested)
            .Should()
            .NotContain(AccessAccount.Create("first@email.test"))
                .And.HaveCount(2);
    }
    
    [Fact]
    public void Remove_RemovesFromViewers_MultipleViewer()
    {
        var tested = CreateDummyFile();
        
        tested.Remove(new HashSet<AccessAccount>() 
            { 
                AccessAccount.Create("first@email.test"),
                AccessAccount.Create("third@email.test")
            });

        GetViewers(tested)
            .Should()
            .NotContain(AccessAccount.Create("first@email.test"))
                .And.NotContain(AccessAccount.Create("third@email.test"))
                .And.HaveCount(1);
    }
    
    [Fact]
    public void Remove_SkipsNonExisting_SingleViewer()
    {
        var tested = CreateDummyFile();
        
        tested.Remove(AccessAccount.Create("non@existing.email"));

        GetViewers(tested)
            .Should()
            .NotContain(AccessAccount.Create("non@existing.email"))
                .And.HaveCount(3);
    }
    
    [Fact]
    public void Remove_SkipsNonExisting_MultipleViewer()
    {
        var tested = CreateDummyFile();
        
        tested.Remove(new HashSet<AccessAccount>() 
        { 
            AccessAccount.Create("non@existing.email"),
            AccessAccount.Create("first@email.test")
        });

        GetViewers(tested)
            .Should()
            .NotContain(AccessAccount.Create("non@existing.email"))
                .And.NotContain(AccessAccount.Create("first@email.test"))
                .And.HaveCount(2);
    }
}