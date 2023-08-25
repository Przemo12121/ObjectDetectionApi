using Application.PipelineBehaviors;
using Application.Responses;
using Domain.AggregateModels.AccessAccountAggregate;
using Domain.AggregateModels.ProcessedFileAggregate;
using Domain.SeedWork.Interfaces;
using MediatR;
using MediatR.Extensions.AttributedBehaviors;

namespace Application.Requests;

[MediatRBehavior(
    typeof(ResourceExistsCheckPipelineBehavior<DownloadProcessedFileQuery, ProcessedFile>),
    order: 1)]
[MediatRBehavior(
    typeof(ReadAccessVerificationPipelineBehavior),
    order: 2)]
public class DownloadProcessedFileQuery : IRequest<IApplicationResponse>, IResourceCommand<ProcessedFile>
{
    public Guid ResourceId { get; }
    public ProcessedFile? Resource { get; set; }
    public AccessAccount Requester { get; }
    
    public DownloadProcessedFileQuery(Guid fileId, AccessAccount requester)
        => (ResourceId, Requester) = (fileId, requester);
}