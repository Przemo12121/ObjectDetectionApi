namespace Domain.UnitTests.UniqueEntityTests;

// ReSharper disable once InconsistentNaming
public class EqualsTests
{
    [Fact]
    public void Equals_True_SameReference()
    {
        var a = new A();
        var b = a;

        a.Equals(b)
            .Should()
            .BeTrue();
    }
    
    [Fact]
    public void Equals_True_SameClassAndId()
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
    public void Equals_False_SameClassAndDifferentId()
    {
        var a = new A();
        var b = new A();
            
        a.Equals(b)
            .Should()
            .BeFalse();
    }
    
    [Fact]
    public void Equals_False_DifferentClassAndSameId()
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