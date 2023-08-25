namespace Application.UnitTests.PipelineBehaviorsTests.DummyRequests;

public sealed record DummyPayloadCommandRequest(string Payload) : IRequest<IApplicationResponse>, IPayloadCommand<string>
{
    
}
