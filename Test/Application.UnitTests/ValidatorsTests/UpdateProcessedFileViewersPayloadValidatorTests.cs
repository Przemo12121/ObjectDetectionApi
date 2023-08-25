namespace Application.UnitTests.ValidatorsTests;

public class UpdateProcessedFileViewersPayloadValidatorTests
{
    private readonly UpdateProcessedFilePayloadValidator _payloadValidator = new();
    
    [Fact]
    public void Validate_ReturnsSuccess_GivenEmptyCollection()
    {
        UpdateProcessedFilePayload payload = new()
        {
            ViewersEmails = new List<string>()
        };

        var result = _payloadValidator.Validate(payload);

        result.IsValid.Should()
            .BeTrue();
    }
    
    [Fact]
    public void Validate_ReturnsSuccess_GivenCollectionOfCorrectEmails()
    {
        UpdateProcessedFilePayload payload = new()
        {
            ViewersEmails = new [] { "aaaa@bbb.ccc", "test.email@test.dev", "aaa.bbb@ccc", "xyz@zyx" }
        };

        var result = _payloadValidator.Validate(payload);

        result.IsValid.Should()
            .BeTrue();
    }
    
    [Fact]
    public void Validate_ReturnsSuccess_GivenCollectionWithDuplicates()
    {
        UpdateProcessedFilePayload payload = new()
        {
            ViewersEmails = new [] { "aaaa@bbb.ccc", "test.email@test.dev", "aaaa@bbb.ccc", "xyz@zyx" }
        };

        var result = _payloadValidator.Validate(payload);

        result.IsValid.Should()
            .BeTrue();
    }
    
    [Theory]
    [InlineData("test.email")]
    [InlineData("test.email@")]
    [InlineData("test@")]
    [InlineData("testemail")]
    [InlineData("@test")]
    [InlineData("@test.email")]
    public void Validate_ReturnsFailure_GivenCollectionWithIncorrectEmail(string incorrectEmail)
    {
        UpdateProcessedFilePayload payload = new()
        {
            ViewersEmails = new [] { "aaaa@bbb.ccc", incorrectEmail, "aaa.bbb@ccc", "xyz@zyx" }
        };

        var result = _payloadValidator.Validate(payload);

        result.IsValid.Should()
            .BeFalse();
    }
}