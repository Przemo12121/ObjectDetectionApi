namespace Application.Requests;

public interface IResourceCommand<T> where T : class
{
    T? Resource { get; set; }
    Guid ResourceId { get; }
}