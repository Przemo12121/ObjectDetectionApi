using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Api;

public static class ProgramExtensions
{
    public static void ConfigureDatabase(this WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<DbContextOptions<ObjectDetectionDbContext>>(
            _ => new DbContextOptionsBuilder<ObjectDetectionDbContext>()
                .UseNpgsql(builder.Configuration.GetConnectionString("ObjectDetectionDb"))
                .LogTo(Console.WriteLine, LogLevel.Information)
                .Options);
        
        builder.Services.AddDbContext<ObjectDetectionDbContext>();
    }
}