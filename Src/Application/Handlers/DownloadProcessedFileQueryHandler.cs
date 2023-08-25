using Application.Requests;
using Application.Responses;
using Domain.AggregateModels;
using Domain.AggregateModels.ProcessedFileAggregate;
using MediatR;

namespace Application.Handlers;

public class DownloadProcessedFileQueryHandler : IRequestHandler<DownloadProcessedFileQuery, IApplicationResponse>
{
    private readonly IFileStorage<ProcessedFile> _fileStorage;

    public DownloadProcessedFileQueryHandler(IFileStorage<ProcessedFile> fileStorage)
        => _fileStorage = fileStorage;

    public Task<IApplicationResponse> Handle(DownloadProcessedFileQuery request, CancellationToken cancellationToken)
    {
        var file = _fileStorage.Read(request.Resource!.StorageData.Uri)!;
        return Task.FromResult<IApplicationResponse>(new DownloadFileResponse(file));
    }
}