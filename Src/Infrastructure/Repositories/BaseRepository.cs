using Infrastructure.Database;

namespace Infrastructure.Repositories;

public abstract class BaseRepository : IDisposable
{
    protected ObjectDetectionDbContext DbContext { get; }
    private bool _disposed = false;

    public BaseRepository(ObjectDetectionDbContext dbContext)
        => DbContext = dbContext;


    private void Dispose(bool disposing)
    {
        if (!_disposed && disposing)
        {
            DbContext.Dispose();
        }
        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}