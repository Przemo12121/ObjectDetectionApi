using Domain.AggregateModels.AccessAccountAggregate;
using Domain.SeedWork.Interfaces;

namespace Domain.AggregateModels;

public interface IUniqueEntityRepository<T>
    where T : UniqueEntity
{
    Task AddAsync(T entity);
    Task RemoveAsync(T entity);
    Task<List<T>> GetManyAsync(AccessAccount owner, Func<IPaginationBuilder<T>, IPaginationBuilder<T>> configurePagination);
    Task<T?> GetAsync(Guid id);
}