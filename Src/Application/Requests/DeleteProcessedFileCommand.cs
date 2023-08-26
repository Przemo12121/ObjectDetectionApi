using Application.PipelineBehaviors;
using Application.Responses;
using Domain.AggregateModels.AccessAccountAggregate;
using Domain.AggregateModels.ProcessedFileAggregate;
using MediatR;
using MediatR.Extensions.AttributedBehaviors;

namespace Application.Requests;

[MediatRBehavior(
    typeof(ResourceExistsCheckPipelineBehavior<DeleteProcessedFileCommand, ProcessedFile>),
    order: 1)]
[MediatRBehavior(
    typeof(OwnerAccessVerificationPipelineBehavior<DeleteProcessedFileCommand, ProcessedFile>),
    order: 2)]
public class DeleteProcessedFileCommand : IRequest<IApplicationResponse>, IModifyCommand<ProcessedFile>
{
    public Guid ResourceId { get; } 
    public AccessAccount Requester { get; }

    public ProcessedFile? Resource { get; set; }
    
    public DeleteProcessedFileCommand(Guid fileId, AccessAccount requester)
        => (ResourceId, Requester) = (fileId, requester);

}