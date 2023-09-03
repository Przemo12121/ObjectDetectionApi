using System.ComponentModel.DataAnnotations;

namespace Domain.AggregateModels.AccessAccountAggregate;

public sealed class AccessAccount : IAccessAccount
{
    public string Id { get; }

#pragma warning disable CS8618
    private AccessAccount() {}
#pragma warning restore CS8618
    
    private AccessAccount(string id) 
        => Id = id;

    public override bool Equals(object? obj)
        => obj is AccessAccount acc && acc.Id.Equals(Id);

    public override int GetHashCode() => Id.GetHashCode();

    public static AccessAccount Create(string email) 
        => new EmailAddressAttribute().IsValid(email) 
            ? new AccessAccount(email.Trim().ToLower()) 
            : throw new ArgumentException("Given string does not match valid email format.");

    public static implicit operator AccessAccount(string str) => 
        Create(str);
    
    public override string ToString() => Id;
}