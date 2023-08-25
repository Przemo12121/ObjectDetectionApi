namespace Application.UnitTests.PipelineBehaviorsTests;

public class PayloadValidationPipelineBehaviorTests : PipelineBehaviorsTests
{
    static readonly ValidationResult SuccessfulValidationResult = new()
    {
        Errors = new List<ValidationFailure>()
    };
    
    [Fact]
    public async void Handle_ReturnsSuccessfulResponse_GivenSuccessfulValidation()
    {
        var validatorMoq = Substitute.For<IValidator<string>>();
        validatorMoq.Validate(Arg.Any<string>())
            .Returns(SuccessfulValidationResult);

        var sut = new PayloadValidationPipelineBehavior<DummyPayloadCommandRequest, string>(
            new List<IValidator<string>>() { validatorMoq });

        var result = await sut.Handle(
            new DummyPayloadCommandRequest("payload"),
            DummyDelegate,
            CancellationToken.None);
        
        result.Should().BeEquivalentTo(SuccessApplicationResponse);
    }
    
    [Fact]
    public async void Handle_ReturnsBadRequestResponse_GivenUnsuccessfulValidation()
    {
        var successfulValidationMoq = Substitute.For<IValidator<string>>();
        successfulValidationMoq.Validate(Arg.Any<string>())
            .Returns(SuccessfulValidationResult);
        var unsuccessfulValidationMoq = Substitute.For<IValidator<string>>();
        unsuccessfulValidationMoq.Validate(Arg.Any<string>())
            .Returns(new ValidationResult()
            {
                Errors = new()
                {
                    new()
                    {
                        PropertyName = "PropertyA",
                        ErrorMessage = "Error Message"
                    }
                }
            });

        var sut = new PayloadValidationPipelineBehavior<DummyPayloadCommandRequest, string>(
            new List<IValidator<string>>() { successfulValidationMoq, unsuccessfulValidationMoq });

        var result = await sut.Handle(
            new DummyPayloadCommandRequest("payload"),
            DummyDelegate,
            CancellationToken.None);
        
        result.Should().BeOfType<BadRequestResponse>();
    }
    
    [Fact]
    public async void Handle_ReturnsGroupedErrorsByProperties_GivenUnsuccessfulValidation()
    {
        var firstValidatorMoq = Substitute.For<IValidator<string>>();
        firstValidatorMoq.Validate(Arg.Any<string>())
            .Returns(new ValidationResult()
            {
                Errors = new()
                {
                    new()
                    {
                        PropertyName = "PropertyA",
                        ErrorMessage = "Error Message A 1"
                    },
                    new()
                    {
                        PropertyName = "PropertyA",
                        ErrorMessage = "Error Message A 2"
                    },
                    new()
                    {
                        PropertyName = "PropertyB",
                        ErrorMessage = "Error Message B 1"
                    }
                }
            });
        var secondValidatorMoq = Substitute.For<IValidator<string>>();
        secondValidatorMoq.Validate(Arg.Any<string>())
            .Returns(new ValidationResult()
            {
                Errors = new()
                {
                    new()
                    {
                        PropertyName = "PropertyB",
                        ErrorMessage = "Error Message B 2"
                    },
                    new()
                    {
                        PropertyName = "PropertyA",
                        ErrorMessage = "Error Message A 3"
                    }
                }
            });

        var sut = new PayloadValidationPipelineBehavior<DummyPayloadCommandRequest, string>(
            new List<IValidator<string>>() { firstValidatorMoq, secondValidatorMoq });

        var result = await sut.Handle(
            new DummyPayloadCommandRequest("payload"),
            DummyDelegate,
            CancellationToken.None);

        ((BadRequestResponse)result).Payload.ErrorList.Should()
            .BeEquivalentTo(new Dictionary<string, List<string>>()
            {
                { "PropertyA", new List<string> { "Error Message A 1", "Error Message A 2", "Error Message A 3" } },
                { "PropertyB", new List<string> { "Error Message B 1", "Error Message B 2" }}
            });
    }
}