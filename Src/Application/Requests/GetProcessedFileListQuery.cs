using Application.PipelineBehaviors;
using Application.Requests.Payloads;
using Application.Responses;
using Domain.AggregateModels;
using Domain.AggregateModels.AccessAccountAggregate;
using Domain.SeedWork.Interfaces;
using MediatR;
using MediatR.Extensions.AttributedBehaviors;

namespace Application.Requests;

[MediatRBehavior(typeof(PayloadValidationPipelineBehavior<GetProcessedFileListQuery, FilePaginationPayload>))]
public class GetProcessedFileListQuery : IRequest<IApplicationResponse>, IPayloadCommand<FilePaginationPayload>
{
    public FilePaginationPayload Payload { get; }
    public AccessAccount Owner { get; }

    public GetProcessedFileListQuery(FilePaginationPayload filePaginationPayload, AccessAccount owner)
        => (Payload, Owner) = (filePaginationPayload, owner);
}