using Domain.AggregateModels.AccessAccountAggregate;
using Domain.SeedWork.Enums;
using Domain.SeedWork.Interfaces;

namespace Domain.AggregateModels;

public interface IFileRepository<T>
    where T : UniqueEntity, IFile
{
    Task AddAsync(T entity);
    Task RemoveAsync(T entity);
    Task<(int totalCount, List<T> files)> GetManyAsync(AccessAccount owner,QueryMediaTypes mediaTypes, Func<IFilePaginationBuilder<T>, IFilePaginationBuilder<T>> configurePagination);
    Task<T?> GetAsync(Guid id);
}