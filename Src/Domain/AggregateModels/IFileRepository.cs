using Domain.AggregateModels.AccessAccountAggregate;
using Domain.SeedWork.Interfaces;

namespace Domain.AggregateModels;

public interface IFileRepository<T>
    where T : UniqueEntity, IFile
{
    Task AddAsync(T entity);
    Task RemoveAsync(T entity);
    Task<List<T>> GetManyAsync(AccessAccount owner, Func<IFilePaginationBuilder<T>, IFilePaginationBuilder<T>> configurePagination);
    Task<T?> GetAsync(Guid id);
}