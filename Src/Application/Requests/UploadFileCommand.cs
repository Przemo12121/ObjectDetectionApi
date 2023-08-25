using Application.PipelineBehaviors;
using Application.Requests.Payloads;
using Application.Responses;
using Domain.AggregateModels.AccessAccountAggregate;
using MediatR;
using MediatR.Extensions.AttributedBehaviors;

namespace Application.Requests;

[MediatRBehavior(typeof(MimeTypeExtractionPipelineBehavior))]
[MediatRBehavior(typeof(PayloadValidationPipelineBehavior<UploadFileCommand, FileStreamPayload>))]
public class UploadFileCommand : IRequest<IApplicationResponse>, IPayloadCommand<FileStreamPayload>
{
    public AccessAccount Requester { get; }
    public FileStreamPayload Payload { get; }

    public UploadFileCommand(FileStreamPayload payload, AccessAccount owner)
        => (Payload, Requester) = (payload, owner);
}