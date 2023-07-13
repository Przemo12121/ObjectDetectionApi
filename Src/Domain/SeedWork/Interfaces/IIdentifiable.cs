namespace Domain.SeedWork.Interfaces;

public interface IIdentifiable<T>
{
    T Id { get; }
}