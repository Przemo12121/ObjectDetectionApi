namespace Application.UnitTests.PipelineBehaviorsTests;

public class ReadAccessVerificationPipelineBehaviorTests : PipelineBehaviorsTests
{
    private static readonly  ProcessedFile DummyFile = new(
        "owner@email.test",
        new("Name", MediaTypes.Image),
        new(FileStorageTypes.LocalStorage, "Uri"),
        new("Url"),
        new List<AccessAccount>()
        {
            "first@viewer.test",
            "second@viewer.test"
        });
    
    [Fact]
    public async void Handle_ReturnsSuccessfulResponse_GivenRequesterTheSameAsOwner()
    {
        var request = new DownloadProcessedFileQuery(DummyFile.Id, "owner@email.test")
        {
            Resource = DummyFile
        };
        
        var sut = new ReadAccessVerificationPipelineBehavior();

        var result = await sut.Handle(request, DummyDelegate, CancellationToken.None);

        result.Should()
            .BeEquivalentTo(SuccessApplicationResponse);
    }
    
    [Fact]
    public async void Handle_ReturnsSuccessfulResponse_GivenRequesterAsOnOfViewer()
    {
        var request = new DownloadProcessedFileQuery(DummyFile.Id, "first@viewer.test")
        {
            Resource = DummyFile
        };
        
        var sut = new ReadAccessVerificationPipelineBehavior();

        var result = await sut.Handle(request, DummyDelegate, CancellationToken.None);

        result.Should()
            .BeEquivalentTo(SuccessApplicationResponse);
    }
    
    [Fact]
    public async void Handle_ReturnsActionForbiddenResponse_GivenRequesterAsNeitherOwnerOrViewer()
    {
        var request = new DownloadProcessedFileQuery(DummyFile.Id, "different@email.test")
        {
            Resource = DummyFile
        };
        
        var sut = new ReadAccessVerificationPipelineBehavior();

        var result = await sut.Handle(request, DummyDelegate, CancellationToken.None);

        result.Should()
            .BeEquivalentTo(new ActionForbiddenResponse(DummyFile.Id, DummyFile.GetType()));
    }
}