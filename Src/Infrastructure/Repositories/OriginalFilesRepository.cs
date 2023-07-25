using Domain.AggregateModels.AccessAccountAggregate;
using Domain.AggregateModels.OriginalFileAggregate;
using Domain.SeedWork.Interfaces;
using Infrastructure.Builders;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public sealed class OriginalFilesRepository : BaseRepository, IOriginalFileRepository
{
    public OriginalFilesRepository(ObjectDetectionDbContext dbContext) : base(dbContext) { }
    
    public async Task AddAsync(OriginalFile entity)
    {
        var accountExists = await UtilQueries.CheckIfExists(DbContext, entity.Owner);
        if (accountExists)
        {
            DbContext.Attach(entity.Owner);
        }
        
        DbContext.OriginalFiles.Add(entity);
        await DbContext.SaveChangesAsync();
    }

    public async Task RemoveAsync(OriginalFile entity)
    {
        DbContext.OriginalFiles.Remove(entity);
        await DbContext.SaveChangesAsync();
    } 

    public Task<List<OriginalFile>> GetManyAsync(AccessAccount owner, Func<IPaginationBuilder<OriginalFile>, IPaginationBuilder<OriginalFile>> configurePagination)
    {
        var query = DbContext.OriginalFiles
            .Where(file => file.Owner.Equals(owner));

        query = configurePagination(
                new FilePaginationBuilder<OriginalFile>(query))
            .Build();
        
        return query.ToListAsync();
    }

    public Task<OriginalFile?> GetAsync(Guid id)
        => DbContext.OriginalFiles
            .Where(file => file.Id.Equals(id))
            .FirstOrDefaultAsync();
}