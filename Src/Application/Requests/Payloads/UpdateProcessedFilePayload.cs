using Application.Constants;

namespace Application.Requests.Payloads;

public sealed record UpdateProcessedFilePayload
{
    public IReadOnlyList<string> ViewersEmails { get; init; } = RequestPayloadDefaults.UpdateProcessedFileViewers.ViewersEmails;
}