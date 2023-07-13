namespace Domain.AggregateModels.AccessAccountAggregate;

public interface IAccessAccountRepository
{
    IAccessAccount GetOrCreate(string email);
    IAccessAccount GetOrCreate(ICollection<string> email);
}