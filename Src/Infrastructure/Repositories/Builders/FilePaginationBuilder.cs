using System.Diagnostics;
using System.Linq.Expressions;
using Domain.AggregateModels;
using Domain.SeedWork.Interfaces;

namespace Infrastructure.Repositories.Builders;

public sealed class FilePaginationBuilder<T> : IFilePaginationBuilder<T>
    where T : UniqueEntity, IFile
{
    private IQueryable<T> Query { get; set; }
    private int Limit { get; set; }
    private int Offset { get; set; }
    private string Order { get; set; } = String.Empty;

    public FilePaginationBuilder(IQueryable<T> query)
        => Query = query;

    public IFilePaginationBuilder<T> ApplyLimit(int limit)
    {
        Limit = limit;
        return this;
    }

    public IFilePaginationBuilder<T> ApplyOffset(int offset)
    {
        Offset = offset;
        return this;
    }
    
    public IFilePaginationBuilder<T> ApplyOrder(string order)
    {
        Order = order.Trim().ToLower();
        return this;
    }

    public IQueryable<T> Build()
    {
        switch (Order.Split(","))
        {
            case []:
                break;
            case [{ } keyValuePair]:
                ApplySingleOrder(keyValuePair);
                break;
            case [{ } firstKeyValuePair, { } secondKeyValuePair]:
                ApplyOrder(firstKeyValuePair, secondKeyValuePair);
                break;
            default:
                throw new UnreachableException($"Unexpected order string: {Order}");
        }

        return Query
            .Skip(Offset)
            .Take(Limit);
    }

    private void ApplySingleOrder(string keyValuePair)
    {
        var str = keyValuePair.Split(":");

        var keySelector = PickKeySelector(str[0]);
        Query = str[1] switch
        {
            "asc" => Query.OrderBy(keySelector),
            "desc" => Query.OrderByDescending(keySelector),
            _ => throw new UnreachableException($"Unexpected order value: {str[1]}")
        };
    }
    
    private void ApplyOrder(string firstKeyValuePair, string secondKeyValuePair)
    {
        var firstStr = firstKeyValuePair.Split(":");
        var secondStr = secondKeyValuePair.Split(":");

        var firstKeySelector = PickKeySelector(firstStr[0]);
        var secondKeySelector = PickKeySelector(secondStr[0]);

        Query = firstStr[1] switch
        {
            "asc" => secondStr[1] switch
            {
                "asc" => Query.OrderBy(firstKeySelector)
                    .ThenBy(secondKeySelector),
                "desc" => Query.OrderBy(firstKeySelector)
                    .ThenByDescending(secondKeySelector),
                _ => throw new UnreachableException($"Unexpected order value: {secondStr[1]}")
            },
            "desc" => secondStr[1] switch
            {
                "asc" => Query.OrderByDescending(firstKeySelector)
                    .ThenBy(secondKeySelector),
                "desc" => Query.OrderByDescending(firstKeySelector)
                    .ThenByDescending(secondKeySelector),
                _ => throw new UnreachableException($"Unexpected order value: {secondStr[1]}")
            },
            _ => throw new UnreachableException($"Unexpected order value: {firstStr[1]}")
        };
    }
    
    private static Expression<Func<T, object>> PickKeySelector(string key)
        => key switch 
        {
            "date" => file => file.CreationDateTime,
            "name" => file => file.Metadata.Name,
            _ => throw new UnreachableException($"Unexpected order key: {key}")
        };
}