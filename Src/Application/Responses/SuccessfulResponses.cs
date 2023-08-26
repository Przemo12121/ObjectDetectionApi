using Application.Responses.Payloads;
using Domain.AggregateModels;
using Domain.SeedWork.Interfaces;

namespace Application.Responses;

public abstract record SuccessfulResponse<T>(T Payload) : IApplicationResponse
{
    public bool Success { get; } = true;
}

public sealed record DownloadFileResponse(FileStream Payload) 
    : SuccessfulResponse<FileStream>(Payload);

public sealed record FileListResponse<T>(FilePaginationPayload<T> Payload) 
    : SuccessfulResponse<FilePaginationPayload<T>>(Payload)
    where T : UniqueEntity, IFile;
    
public sealed record OperationSuccessfulResponse(MessagePayload Payload)
    : SuccessfulResponse<MessagePayload>(Payload);