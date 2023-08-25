using Domain.AggregateModels;
using Domain.SeedWork.Interfaces;

namespace Application.Responses.Payloads;

public sealed record FilePaginationPayload<T>
    where T : UniqueEntity, IFile
{
    public int TotalCount { get; init; }
    public IReadOnlyList<T> Files { get; init; } = null!;
}