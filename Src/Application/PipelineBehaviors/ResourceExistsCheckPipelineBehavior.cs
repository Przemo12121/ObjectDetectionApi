using Application.Requests;
using Application.Responses;
using Domain.AggregateModels;
using Domain.SeedWork.Interfaces;
using MediatR;

namespace Application.PipelineBehaviors;

public class ResourceExistsCheckPipelineBehavior<TRequest, TResource> : IPipelineBehavior<TRequest, IApplicationResponse>
    where TRequest : IResourceCommand<TResource>
    where TResource : UniqueEntity, IFile
{
    private readonly IFileRepository<TResource> _fileRepository;

    public ResourceExistsCheckPipelineBehavior(IFileRepository<TResource> fileRepository)
        => _fileRepository = fileRepository;

    public async Task<IApplicationResponse> Handle(TRequest request, RequestHandlerDelegate<IApplicationResponse> next, CancellationToken cancellationToken)
    {
        request.Resource = await _fileRepository.GetAsync(request.ResourceId);

        return request.Resource is not null
            ? await next()
            : new FileNotFoundResponse(request.ResourceId, typeof(TResource));
    }
}