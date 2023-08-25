using Application.Requests;
using Application.Responses;
using Domain.AggregateModels;
using Domain.AggregateModels.ProcessedFileAggregate;
using MediatR;

namespace Application.Handlers;

public class GetProcessedListQueryHandler : IRequestHandler<GetProcessedFileListQuery, IApplicationResponse>
{
    private readonly IFileRepository<ProcessedFile> _fileRepository;

    public GetProcessedListQueryHandler(IFileRepository<ProcessedFile> fileRepository)
        => _fileRepository = fileRepository;

    public async Task<IApplicationResponse> Handle(GetProcessedFileListQuery request, CancellationToken cancellationToken)
    {
        var (totalCount, files) = await _fileRepository.GetManyAsync(
            request.Owner,
            request.Payload.QueryMediaTypes,
            builder => builder.ApplyOrder(request.Payload.Order)
                .ApplyOffset(request.Payload.Offset)
                .ApplyLimit(request.Payload.Limit));

        return new FileListResponse<ProcessedFile>(new ()
        {
            TotalCount = totalCount,
            Files = files
        });
    }
}