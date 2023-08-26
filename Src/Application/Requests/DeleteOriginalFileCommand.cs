using Application.PipelineBehaviors;
using Application.Responses;
using Domain.AggregateModels;
using Domain.AggregateModels.AccessAccountAggregate;
using Domain.AggregateModels.OriginalFileAggregate;
using Domain.SeedWork.Interfaces;
using MediatR;
using MediatR.Extensions.AttributedBehaviors;

namespace Application.Requests;

[MediatRBehavior(
    typeof(ResourceExistsCheckPipelineBehavior<DeleteOriginalFileCommand, OriginalFile>),
    order: 1)]
[MediatRBehavior(
    typeof(OwnerAccessVerificationPipelineBehavior<DeleteOriginalFileCommand, OriginalFile>),
    order: 2)]
public class DeleteOriginalFileCommand : IRequest<IApplicationResponse>, IModifyCommand<OriginalFile>
{
    public Guid ResourceId { get; } 
    public AccessAccount Requester { get; }

    public OriginalFile? Resource { get; set; }
    
    public DeleteOriginalFileCommand(Guid fileId, AccessAccount requester)
        => (ResourceId, Requester) = (fileId, requester);

}