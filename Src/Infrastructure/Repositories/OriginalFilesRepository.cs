using Domain.AggregateModels.AccessAccountAggregate;
using Domain.AggregateModels.OriginalFileAggregate;
using Domain.SeedWork.Enums;
using Domain.SeedWork.Interfaces;
using Infrastructure.Database;
using Infrastructure.Repositories.Builders;
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
    
    public async Task<(int totalCount, List<OriginalFile> files)> GetManyAsync(AccessAccount owner, QueryMediaTypes mediaTypes, Func<IFilePaginationBuilder<OriginalFile>, IFilePaginationBuilder<OriginalFile>> configurePagination)
    {
        var query = DbContext.OriginalFiles
            .Where(file => file.Owner.Equals(owner));
            
        query = mediaTypes switch
        {
            QueryMediaTypes.Images => query.Where(file => file.Metadata.Type.Equals(MediaTypes.Image)),
            QueryMediaTypes.Videos => query.Where(file => file.Metadata.Type.Equals(MediaTypes.Video)),
            _ => query
        };

        query = query.Include(file => file.Owner);
        
        var totalCount = await query.CountAsync();
        var files = await configurePagination(new FilePaginationBuilder<OriginalFile>(query))
            .Build()
            .ToListAsync();
        
        return (totalCount, files);
    }

    public Task<OriginalFile?> GetAsync(Guid id)
        => DbContext.OriginalFiles
            .Where(file => file.Id.Equals(id))
            .FirstOrDefaultAsync();
}