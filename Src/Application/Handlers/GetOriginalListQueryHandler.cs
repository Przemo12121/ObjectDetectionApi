using Application.Requests;
using Application.Responses;
using Domain.AggregateModels;
using Domain.AggregateModels.OriginalFileAggregate;
using MediatR;

namespace Application.Handlers;

public class GetOriginalListQueryHandler : IRequestHandler<GetOriginalFileListQuery, IApplicationResponse>
{
    private readonly IFileRepository<OriginalFile> _fileRepository;

    public GetOriginalListQueryHandler(IFileRepository<OriginalFile> fileRepository)
        => _fileRepository = fileRepository;

    public async Task<IApplicationResponse> Handle(GetOriginalFileListQuery request, CancellationToken cancellationToken)
    {
        var (totalCount, files) = await _fileRepository.GetManyAsync(
            request.Owner,
            request.Payload.QueryMediaTypes,
            builder => builder.ApplyOrder(request.Payload.Order)
                .ApplyOffset(request.Payload.Offset)
                .ApplyLimit(request.Payload.Limit));

        return new FileListResponse<OriginalFile>(new ()
        {
            TotalCount = totalCount,
            Files = files
        });
    }
}