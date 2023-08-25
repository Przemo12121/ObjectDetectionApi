using Application.Constants;
using Domain.SeedWork.Enums;

namespace Application.Requests.Payloads;

public sealed record FilePaginationPayload
{
    public int Limit { get; init; } = RequestPayloadDefaults.FilePagination.Limit;
    public int Offset { get; init; } = RequestPayloadDefaults.FilePagination.Offset;
    public string Order { get; init; } = RequestPayloadDefaults.FilePagination.Order;
    public QueryMediaTypes QueryMediaTypes { get; init; } = RequestPayloadDefaults.FilePagination.QueryMediaTypes;
}
