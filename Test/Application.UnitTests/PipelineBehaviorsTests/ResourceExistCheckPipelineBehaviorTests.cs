namespace Application.UnitTests.PipelineBehaviorsTests;

public class ResourceExistCheckPipelineBehaviorTests : PipelineBehaviorsTests
{
    [Fact]
    public async void Handle_ReturnsNotFoundResponse_GivenNonExistingId()
    {
        var id = Guid.NewGuid();
        var repositoryMoq = Substitute.For<IFileRepository<OriginalFile>>();
        repositoryMoq.GetAsync(Arg.Any<Guid>())
            .Returns((OriginalFile?)null);

        var sut = new ResourceExistsCheckPipelineBehavior<DummyModifyRequest, OriginalFile>(repositoryMoq);

        var result = await sut.Handle(
            new DummyModifyRequest(id, "dummy@email.test"),
            DummyDelegate,
            CancellationToken.None);

        result.Should()
            .BeEquivalentTo(new FileNotFoundResponse(id, typeof(OriginalFile)));
    }
    
    [Fact]
    public async void Handle_ReturnsSuccessfulResponse_GivenExistingId()
    {
        var repositoryMoq = Substitute.For<IFileRepository<OriginalFile>>();
        repositoryMoq.GetAsync(Arg.Any<Guid>())
            .Returns(new OriginalFile(
                new("Name", MediaTypes.Image),
                new(FileStorageTypes.LocalStorage, "Uri"),
                "dummy@email.test"));

        var sut = new ResourceExistsCheckPipelineBehavior<DummyModifyRequest, OriginalFile>(repositoryMoq);

        var result = await sut.Handle(
            new DummyModifyRequest(Guid.NewGuid(), "dummy@email.test"),
            DummyDelegate,
            CancellationToken.None);

        result.Should()
            .BeEquivalentTo(SuccessApplicationResponse);
    }
    
    [Fact]
    public async void Handle_InitializesRequestFile_GivenExistingId()
    {
        var request = new DummyModifyRequest(Guid.NewGuid(), "dummy@email.test");
        var repositoryMoq = Substitute.For<IFileRepository<OriginalFile>>();
        repositoryMoq.GetAsync(Arg.Any<Guid>())
            .Returns(new OriginalFile(
                new("Name", MediaTypes.Image),
                new(FileStorageTypes.LocalStorage, "Uri"),
                "dummy@email.test"));

        var sut = new ResourceExistsCheckPipelineBehavior<DummyModifyRequest, OriginalFile>(repositoryMoq);

        await sut.Handle(request, DummyDelegate, CancellationToken.None);

        request.Resource.Should().NotBeNull();
    }
}