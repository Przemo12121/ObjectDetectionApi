namespace Domain.UnitTests.AccessAccountTests;

public class CreateTests
{
    [Theory]
    [InlineData("test123@email.test")]
    [InlineData("test@email")]
    [InlineData("test.test@email123")]
    [InlineData("test@email.test")]
    [InlineData("test.email@email.test123")]
    [InlineData("test.email123@email.test")]
    public void Create_NotThrowsException_GivenValidEmailAddress(string tested)
    {
        Action action = () => AccessAccount.Create(tested);

        action.Should()
            .NotThrow<ArgumentException>();
    }
    
    [Fact]
    public void Create_ReturnsAccessAccount_GivenValidEmailAddress()
    {
        var result = AccessAccount.Create("test@email.test");

        result.Should()
            .NotBeNull();
    }
    
    [Fact]
    public void Create_CreatesAccessAccountWithCorrectEmail_GivenValidEmailAddress()
    {
        var result = AccessAccount.Create("   teST@emaIL.teSt   ");

        result.Id
            .Should()
            .Be("test@email.test");
    }
    
    [Theory]
    [InlineData("not valid email")]
    [InlineData("test@email@test")]
    [InlineData("test@")]
    [InlineData("test.email@")]
    [InlineData("@email")]
    [InlineData("@email.test")]
    [InlineData("test.email123@@email.test")]
    public void Create_ThrowsInvalidArgumentException_GivenInvalidEmailAddress(string tested)
    {
        Action action = () => AccessAccount.Create(tested);

        action.Should()
            .Throw<ArgumentException>();                
    }
}