namespace Domain.UnitTests.ProcessedFileTests;

public class AddTests : ProcessedFileTest
{
    [Fact]
    public void Add_AddsToViewers_SingleViewer()
    {
        var tested = CreateDummyFile();
        
        tested.Add("fourth@email.test");

        GetViewers(tested)
            .Should()
            .Contain("fourth@email.test")
                .And.HaveCount(4);
    }
    
    [Fact]
    public void Add_AddsToViewers_MultipleViewers()
    {
        var tested = CreateDummyFile();
        
        tested.Add(new HashSet<AccessAccount>() 
            { 
                "fifth@email.test",
                "fourth@email.test",
                "fifth@email.test"
            });

        GetViewers(tested)
            .Should()
            .Contain("fourth@email.test")
                .And.Contain("fifth@email.test")
                .And.HaveCount(5);
    }
    
    [Fact]
    public void Add_SkipsAlreadyExisting_SingleViewer()
    {
        var tested = CreateDummyFile();
        
        tested.Add("first@email.test");

        GetViewers(tested)
            .Should()
            .Contain("first@email.test")
                .And.HaveCount(3);
    }
    
    [Fact]
    public void Add_SkipsAlreadyExisting_MultipleViewers()
    {
        var tested = CreateDummyFile();
        
        tested.Add(new HashSet<AccessAccount>() 
        { 
            "fourth@email.test",
            "first@email.test"
        });

        GetViewers(tested)
            .Should()
            .Contain("fourth@email.test")
                .And.Contain("first@email.test")
                .And.HaveCount(4);
    }
}