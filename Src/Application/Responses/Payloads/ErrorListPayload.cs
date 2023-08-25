namespace Application.Responses.Payloads;

public sealed record ErrorListPayload
{
    public Dictionary<string, List<string>> ErrorList { get; init; } = null!;
}