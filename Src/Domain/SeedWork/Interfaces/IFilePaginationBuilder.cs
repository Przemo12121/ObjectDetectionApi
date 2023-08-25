using Domain.AggregateModels;
using Domain.SeedWork.Enums;

namespace Domain.SeedWork.Interfaces;

public interface IFilePaginationBuilder<T> 
    where T : UniqueEntity, IFile
{
    IFilePaginationBuilder<T> ApplyLimit(int limit);
    IFilePaginationBuilder<T> ApplyOffset(int offset);
    IFilePaginationBuilder<T> ApplyOrder(string order);
    IQueryable<T> Build();
}
