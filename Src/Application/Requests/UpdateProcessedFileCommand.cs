using Application.PipelineBehaviors;
using Application.Requests.Payloads;
using Application.Responses;
using Domain.AggregateModels.AccessAccountAggregate;
using Domain.AggregateModels.ProcessedFileAggregate;
using MediatR;
using MediatR.Extensions.AttributedBehaviors;

namespace Application.Requests;

[MediatRBehavior(
    typeof(PayloadValidationPipelineBehavior<UpdateProcessedFileCommand, UpdateProcessedFilePayload>),
    order: 1)]
[MediatRBehavior(
    typeof(ResourceExistsCheckPipelineBehavior<UpdateProcessedFileCommand, ProcessedFile>),
    order: 2)]
[MediatRBehavior(
    typeof(OwnerAccessVerificationPipelineBehavior<UpdateProcessedFileCommand, ProcessedFile>),
    order: 3)]
public class UpdateProcessedFileCommand : IRequest<IApplicationResponse>, IModifyCommand<ProcessedFile>, IPayloadCommand<UpdateProcessedFilePayload>
{
    public UpdateProcessedFilePayload Payload { get; }
    public Guid ResourceId { get; }
    public ProcessedFile? Resource { get; set; }
    public AccessAccount Requester { get; }

    public UpdateProcessedFileCommand(Guid fileId, UpdateProcessedFilePayload payload, AccessAccount requester)
        => (ResourceId, Payload, Requester) = (fileId, payload, requester);
}