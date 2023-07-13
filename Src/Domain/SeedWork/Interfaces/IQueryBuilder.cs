using Domain.AggregateModels;

namespace Domain.SeedWork.Interfaces;

public interface IQueryBuilder<T> 
    where T : UniqueEntity
{
    IQueryable<T> Build();
}