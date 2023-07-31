namespace Domain.UnitTests.UniqueEntityTests;

public class EqualsTests
{
    [Fact]
    public void Equals_ReturnsTrue_GivenSameReference()
    {
        var a = new A();
        var b = a;

        a.Equals(b)
            .Should()
            .BeTrue();
    }
    
    [Fact]
    public void Equals_ReturnsTrue_GivenSameClassAndId()
    {
        var a = new A();
        var b = new A();
        
        typeof(UniqueEntity)
            .GetField("<Id>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic)!
            .SetValue(b, Guid.Parse(a.Id.ToString()));
        
        a.Equals(b)
            .Should()
            .BeTrue();
    }
    
    [Fact]
    public void Equals_ReturnsFalse_GivenSameClassAndDifferentId()
    {
        var a = new A();
        var b = new A();
            
        a.Equals(b)
            .Should()
            .BeFalse();
    }
    
    [Fact]
    public void Equals_ReturnsFalse_GivenDifferentClassAndSameId()
    {
        var a = new A();
        var b = new B();
        
        typeof(UniqueEntity)
            .GetField("<Id>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic)!
            .SetValue(b, Guid.Parse(a.Id.ToString()));
        
        // ReSharper disable once SuspiciousTypeConversion.Global
        a.Equals(b)
            .Should()
            .BeFalse();
    }
}

public class A : UniqueEntity { }
public class B : UniqueEntity { }