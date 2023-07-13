using Domain.AggregateModels.AccessAccountAggregate;

namespace Domain.SeedWork.Interfaces;

public interface IOwnable<T>
    where T : IAccessAccount
{
    T Owner { get; }
}