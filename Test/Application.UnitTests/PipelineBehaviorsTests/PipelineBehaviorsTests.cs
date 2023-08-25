namespace Application.UnitTests.PipelineBehaviorsTests;

public abstract class PipelineBehaviorsTests
{
    protected static readonly OperationSuccessfulResponse SuccessApplicationResponse = new("Dummy message");
    
    protected static Task<IApplicationResponse> DummyDelegate()
        => Task.FromResult<IApplicationResponse>(SuccessApplicationResponse);
}