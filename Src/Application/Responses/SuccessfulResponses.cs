using Application.Responses.Payloads;
using Domain.AggregateModels;
using Domain.SeedWork.Interfaces;

namespace Application.Responses;

public abstract record SuccessfulResponses<T>(T Payload) : IApplicationResponse
{
    public bool Success { get; } = true;
}

public sealed record DownloadFileResponse(FileStream Payload) 
    : SuccessfulResponses<FileStream>(Payload);

public sealed record FileListResponse<T>(FilePaginationPayload<T> Payload) 
    : SuccessfulResponses<FilePaginationPayload<T>>(Payload)
    where T : UniqueEntity, IFile;
    
public sealed record OperationSuccessfulResponse(MessagePayload Payload)
    : SuccessfulResponses<MessagePayload>(Payload);