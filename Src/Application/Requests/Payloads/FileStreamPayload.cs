using Domain.AggregateModels;
using Domain.SeedWork.Enums;

namespace Application.Requests.Payloads;

public record FileStreamPayload(Stream Stream, string FileName)
{
    public List<string> MimeTypes { get; set; } = Enumerable.Empty<string>().ToList();
};