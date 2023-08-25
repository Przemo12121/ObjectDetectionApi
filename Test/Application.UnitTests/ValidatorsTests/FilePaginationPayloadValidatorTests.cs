namespace Application.UnitTests.ValidatorsTests;

public class FilePaginationPayloadValidatorTests
{
    private readonly FilePaginationPayload _validPayload = new()
    {
        Limit = 20,
        Offset = 10,
        QueryMediaTypes = QueryMediaTypes.All,
        Order = "date:asc,name:desc"
    };
    
    private readonly FilePaginationPayloadValidator _payloadValidator = new();

    [Fact]
    public void Validate_ReturnsSuccess_GivenValidPayload()
    {
        var result = _payloadValidator.Validate(_validPayload);
        
        result.IsValid.Should()
            .BeTrue();
    }
    
    [Fact]
    public void Validate_ReturnsFailure_GivenLimitUnder1()
    {
        var payload = _validPayload with { Limit = 0 };
        
        var result = _payloadValidator.Validate(payload);
        
        result.IsValid.Should()
            .BeFalse();
    }
    
    [Fact]
    public void Validate_ReturnsFailure_GivenLimitOver100()
    {
        var payload = _validPayload with { Limit = 101 };
        
        var result = _payloadValidator.Validate(payload);
        
        result.IsValid.Should()
            .BeFalse();
    }
    
    [Fact]
    public void Validate_ReturnsFailure_GivenLimitOf100()
    {
        var payload = _validPayload with { Limit = 100 };
        
        var result = _payloadValidator.Validate(payload);
        
        result.IsValid.Should()
            .BeTrue();
    }
    
    [Fact]
    public void Validate_ReturnsFailure_GivenLimitOf1()
    {
        var payload = _validPayload with { Limit = 1 };
        
        var result = _payloadValidator.Validate(payload);
        
        result.IsValid.Should()
            .BeTrue();
    }
    
    [Fact]
    public void Validate_ReturnsFailure_GivenOffsetOf0()
    {
        var payload = _validPayload with { Offset = 0 };
        
        var result = _payloadValidator.Validate(payload);
        
        result.IsValid.Should()
            .BeTrue();
    }
    
    [Fact]
    public void Validate_ReturnsFailure_GivenOffsetOfGreatValue()
    {
        var payload = _validPayload with { Offset = 123091231 };
        
        var result = _payloadValidator.Validate(payload);
        
        result.IsValid.Should()
            .BeTrue();
    }
    
    [Fact]
    public void Validate_ReturnsFailure_GivenOffsetUnder0()
    {
        var payload = _validPayload with { Offset = -1 };
        
        var result = _payloadValidator.Validate(payload);
        
        result.IsValid.Should()
            .BeFalse();
    }
    
    [Fact]
    public void Validate_ReturnsSuccess_GivenOrderOfSingleKeyValuePair()
    {
        var payload = _validPayload with { Order = "name:asc" };
        
        var result = _payloadValidator.Validate(payload);
        
        result.IsValid.Should()
            .BeTrue();
    }
    
    [Fact]
    public void Validate_ReturnsSuccess_GivenOrderOfTwoKeyValuePairs()
    {
        var payload = _validPayload with { Order = "name:asc,date:desc" };
        
        var result = _payloadValidator.Validate(payload);
        
        result.IsValid.Should()
            .BeTrue();
    }
    
    [Fact]
    public void Validate_ReturnsSuccess_GivenOrderOfThreeKeyValuePairs()
    {
        var payload = _validPayload with { Order = "name:asc,date:desc,name:desc" };
        
        var result = _payloadValidator.Validate(payload);
        
        result.IsValid.Should()
            .BeFalse();
    }
    
    [Fact]
    public void Validate_ReturnsSuccess_GivenOrderOfEmptyString()
    {
        var payload = _validPayload with { Order = String.Empty };
        
        var result = _payloadValidator.Validate(payload);
        
        result.IsValid.Should()
            .BeFalse();
    }
    
    [Fact]
    public void Validate_ReturnsSuccess_GivenOrderOfNotSupportedKey()
    {
        var payload = _validPayload with { Order = "field:asc" };
        
        var result = _payloadValidator.Validate(payload);
        
        result.IsValid.Should()
            .BeFalse();
    }
    
    [Fact]
    public void Validate_ReturnsSuccess_GivenOrderOfNotSupportedValue()
    {
        var payload = _validPayload with { Order = "name:value" };
        
        var result = _payloadValidator.Validate(payload);
        
        result.IsValid.Should()
            .BeFalse();
    }
    
    [Fact]
    public void Validate_ReturnsSuccess_GivenOrderOfKeyWithoutValue()
    {
        var payload = _validPayload with { Order = "name" };
        
        var result = _payloadValidator.Validate(payload);
        
        result.IsValid.Should()
            .BeFalse();
    }
    
    [Fact]
    public void Validate_ReturnsSuccess_GivenOrderOfValueWithoutKey()
    {
        var payload = _validPayload with { Order = "value" };
        
        var result = _payloadValidator.Validate(payload);
        
        result.IsValid.Should()
            .BeFalse();
    }
    
    [Theory]
    [InlineData("name,asc")]
    [InlineData("name-asc")]
    [InlineData("name asc")]
    public void Validate_ReturnsSuccess_GivenOrderWithBadFormat(string order)
    {
        var payload = _validPayload with { Order = order };
        
        var result = _payloadValidator.Validate(payload);
        
        result.IsValid.Should()
            .BeFalse();
    }
    
    [Fact]
    public void Validate_ReturnsSuccess_GivenOrderOfDuplicatedKeys()
    {
        var payload = _validPayload with { Order = "name:asc,name:desc" };
        
        var result = _payloadValidator.Validate(payload);
        
        result.IsValid.Should()
            .BeFalse();
    }
}