namespace Application.Requests;

public interface IPayloadCommand<T> where T : class
{
    T Payload { get; }
}