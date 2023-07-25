using Domain.AggregateModels.AccessAccountAggregate;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

internal static class UtilQueries
{
    public static async Task<bool> CheckIfExists(ObjectDetectionDbContext dbContext, AccessAccount account)
    {
        var existing = await dbContext.AccessAccounts
            .AsNoTracking()
            .Where(acc => acc.Equals(account))
            .FirstOrDefaultAsync();

        return existing is not null;
    }
    
    public static Task<List<AccessAccount>> GetExistingAccounts(ObjectDetectionDbContext dbContext, List<AccessAccount> accounts)
        => dbContext.AccessAccounts
            .AsNoTracking()
            .Where(account => accounts.Contains(account))
            .ToListAsync();
}