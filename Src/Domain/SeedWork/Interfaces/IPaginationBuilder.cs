using Domain.AggregateModels;

namespace Domain.SeedWork.Interfaces;

public interface IPaginationBuilder<T> 
    where T : UniqueEntity
{
    IPaginationBuilder<T> ApplyLimit(int limit);
    IPaginationBuilder<T> ApplyOffset(int offset);
    IPaginationBuilder<T> ApplyOrder(string order);
    IQueryable<T> Build();
}
