namespace Application.UnitTests.PipelineBehaviorsTests.DummyRequests;

public sealed record DummyModifyRequest(Guid ResourceId, AccessAccount Requester) : IRequest<IApplicationResponse>, IModifyCommand<OriginalFile>
{
    public OriginalFile? Resource { get; set; }
}