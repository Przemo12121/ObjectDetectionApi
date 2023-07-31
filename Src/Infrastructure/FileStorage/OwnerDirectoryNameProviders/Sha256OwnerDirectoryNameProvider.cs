using System.Security.Cryptography;
using System.Text;
using Domain.AggregateModels.AccessAccountAggregate;

namespace Infrastructure.FileStorage.OwnerDirectoryNameProviders;

public class Sha256OwnerDirectoryNameProvider : IOwnerDirectoryNameProvider, IDisposable
{
    private readonly SHA256 _hasher = SHA256.Create();

    public string GetFor(AccessAccount owner)
        => Convert.ToHexString(
            Encoding.UTF8.GetBytes(owner.Id));

    public void Dispose()
        => _hasher.Dispose();
}