namespace Domain.UnitTests.ProcessedFileTests;

public class AddTests : ProcessedFileTest
{
    [Fact]
    public void Add_AddsToViewers_SingleViewer()
    {
        var tested = CreateDummyFile();
        
        tested.Add(AccessAccount.Create("fourth@email.test"));

        GetViewers(tested)
            .Should()
            .Contain(AccessAccount.Create("fourth@email.test"))
                .And.HaveCount(4);
    }
    
    [Fact]
    public void Add_AddsToViewers_MultipleViewers()
    {
        var tested = CreateDummyFile();
        
        tested.Add(new HashSet<AccessAccount>() 
            { 
                AccessAccount.Create("fifth@email.test"),
                AccessAccount.Create("fourth@email.test"),
                AccessAccount.Create("fifth@email.test")
            });

        GetViewers(tested)
            .Should()
            .Contain(AccessAccount.Create("fourth@email.test"))
                .And.Contain(AccessAccount.Create("fifth@email.test"))
                .And.HaveCount(5);
    }
    
    [Fact]
    public void Add_SkipsAlreadyExisting_SingleViewer()
    {
        var tested = CreateDummyFile();
        
        tested.Add(AccessAccount.Create("first@email.test"));

        GetViewers(tested)
            .Should()
            .Contain(AccessAccount.Create("first@email.test"))
                .And.HaveCount(3);
    }
    
    [Fact]
    public void Add_SkipsAlreadyExisting_MultipleViewers()
    {
        var tested = CreateDummyFile();
        
        tested.Add(new HashSet<AccessAccount>() 
        { 
            AccessAccount.Create("fourth@email.test"),
            AccessAccount.Create("first@email.test")
        });

        GetViewers(tested)
            .Should()
            .Contain(AccessAccount.Create("fourth@email.test"))
                .And.Contain(AccessAccount.Create("first@email.test"))
                .And.HaveCount(4);
    }
}