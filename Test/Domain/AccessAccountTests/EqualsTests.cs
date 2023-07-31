namespace Domain.UnitTests.AccessAccountTests;

public class EqualsTests
{
    [Fact]
    public void Equals_ReturnsTrue_GivenSameReference()
    {
        var a = AccessAccount.Create("first@email.test");
        var b = a;

        a.Equals(b)
            .Should()
            .BeTrue();
    }
    
    [Fact]
    public void Equals_True_SameEmail()
    {
        var a = AccessAccount.Create("first@email.test");
        var b = AccessAccount.Create("first@email.test");
        
        a.Equals(b)
            .Should()
            .BeTrue();
    }
    
    [Fact]
    public void Equals_ReturnsTrue_GivenSameEmailWithDifferentCasing()
    {
        var a = AccessAccount.Create("fiRSt@eMaiL.tEst");
        var b = AccessAccount.Create("FirsT@EmaIl.teST");
        
        a.Equals(b)
            .Should()
            .BeTrue();
    }
    
    [Fact]
    public void Equals_ReturnsFalse_GivenDifferentEmail()
    {
        var a = AccessAccount.Create("first@email.test");
        var b = AccessAccount.Create("first@email.test2");
            
        a.Equals(b)
            .Should()
            .BeFalse();
    }
}