using Domain.AggregateModels.AccessAccountAggregate;

namespace Domain.SeedWork.Interfaces;

public interface IViewable<T>
    where T : IAccessAccount
{
    IReadOnlySet<T> Viewers { get; }
}