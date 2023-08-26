namespace Application.UnitTests.PipelineBehaviorsTests;

public class OwnerAccessVerificationPipelineBehaviorTests : PipelineBehaviorsTests
{
    private static readonly OriginalFile DummyFile = new(
        new("Name", MediaTypes.Image),
        new(FileStorageTypes.LocalStorage, "Uri"),
        "owner@email.test");
    
    [Fact]
    public async void Handle_ReturnsSuccessfulResponse_GivenRequesterTheSameAsOwner()
    {
        var request = new DummyModifyRequest(Guid.NewGuid(), "owner@email.test") { Resource = DummyFile };
        
        var sut = new OwnerAccessVerificationPipelineBehavior<DummyModifyRequest, OriginalFile>();

        var result = await sut.Handle(request, DummyDelegate, CancellationToken.None);

        result.Should()
            .BeEquivalentTo(SuccessApplicationResponse);
    }
    
    [Fact]
    public async void Handle_ReturnsActionForbiddenResponse_GivenRequesterAsOnOfViewer()
    {
        var request = new DummyModifyRequest(Guid.NewGuid(), "first@viewer.test") { Resource = DummyFile };
        
        var sut = new OwnerAccessVerificationPipelineBehavior<DummyModifyRequest, OriginalFile>();

        var result = await sut.Handle(request, DummyDelegate, CancellationToken.None);

        result.Should()
            .BeEquivalentTo(new ActionForbiddenResponse(DummyFile.Id, DummyFile.GetType()));
    }
    
    [Fact]
    public async void Handle_ReturnsActionForbiddenResponse_GivenRequesterAsNeitherOwnerOrViewer()
    {
        var request = new DummyModifyRequest(Guid.NewGuid(), "different@email.test") { Resource = DummyFile };
        
        var sut = new OwnerAccessVerificationPipelineBehavior<DummyModifyRequest, OriginalFile>();

        var result = await sut.Handle(request, DummyDelegate, CancellationToken.None);

        result.Should()
            .BeEquivalentTo(new ActionForbiddenResponse(DummyFile.Id, DummyFile.GetType()));
    }
}