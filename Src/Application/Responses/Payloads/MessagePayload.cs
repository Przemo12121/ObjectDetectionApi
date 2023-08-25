namespace Application.Responses.Payloads;

public sealed record MessagePayload(string Message)
{
    public static implicit operator MessagePayload(string message) => new(message);
}