using System.ComponentModel.DataAnnotations;

namespace Domain.AggregateModels.AccessAccountAggregate;

public class AccessAccount : IAccessAccount
{
    public string Id { get; }

    private AccessAccount(string email) => Id = email;

    public override bool Equals(object? obj)
        => obj is AccessAccount acc && acc.Id.Equals(Id);

    public override int GetHashCode() => Id.GetHashCode();

    public static AccessAccount Create(string email)
        => new EmailAddressAttribute().IsValid(email) 
            ? new AccessAccount(email.Trim().ToLower()) 
            : throw new ArgumentException("Given string does not match valid email format.");
}