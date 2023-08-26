using Application.Constants;
using Application.Responses.Payloads;

namespace Application.Responses;

public abstract record UnsuccessfulResponse<T>(T Payload) : IApplicationResponse
{
    public bool Success { get; } = false;
}

public sealed record FileNotFoundResponse(Guid Id, Type Type)
    : UnsuccessfulResponse<MessagePayload>(
        new MessagePayload(ResponseMessages.Errors.FileNotFound(Id, Type)));

public sealed record ActionForbiddenResponse(Guid Id, Type Type)
    : UnsuccessfulResponse<MessagePayload>(
        new MessagePayload(ResponseMessages.Errors.ActionForbidden(Id, Type)));
    
public sealed record BadRequestResponse(ErrorListPayload Payload)
    : UnsuccessfulResponse<ErrorListPayload>(Payload);
