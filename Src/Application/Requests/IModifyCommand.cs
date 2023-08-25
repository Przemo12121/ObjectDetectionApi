using Domain.AggregateModels.AccessAccountAggregate;
using Domain.SeedWork.Interfaces;

namespace Application.Requests;

public interface IModifyCommand<T> : IResourceCommand<T>
    where T : class, IIdentifiable<Guid>, IOwnable<AccessAccount>
{
    AccessAccount Requester { get; }
}