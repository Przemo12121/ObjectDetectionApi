using Domain.SeedWork.Interfaces;

namespace Domain.AggregateModels;

public interface IUniqueEntityRepository<T>
    where T : UniqueEntity
{
    void Add(T originalFile);
    void Remove(T originalFile);
    IReadOnlyList<T> GetMany(IQueryBuilder<T> queryBuilder);
    T? Get(Guid id);
}