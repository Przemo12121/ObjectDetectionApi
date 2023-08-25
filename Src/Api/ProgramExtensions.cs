using System.Reflection;
using Application.Requests;
using Application.Responses;
using Application.Services.MqttServices;
using Domain.AggregateModels;
using Domain.AggregateModels.OriginalFileAggregate;
using Domain.AggregateModels.ProcessedFileAggregate;
using Infrastructure.Database;
using Infrastructure.FileStorage;
using Infrastructure.FileStorage.OwnerDirectoryNameProviders;
using Infrastructure.Repositories;
using MediatR.Extensions.AttributedBehaviors;
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

    public static void ConfigureFileStorages(this WebApplicationBuilder builder)
    {
        var originalFilesStorageDirectoryPath = builder.Configuration
            .GetSection("LocalStorage")
            .GetSection("OriginalFiles")
            .Value;
        if (originalFilesStorageDirectoryPath is null)
        {
            throw new ArgumentNullException(nameof(originalFilesStorageDirectoryPath));
        }
        
        var processedFilesStorageDirectoryPath = builder.Configuration
            .GetSection("LocalStorage")
            .GetSection("ProcessedFiles")
            .Value;
        if (processedFilesStorageDirectoryPath is null)
        {
            throw new ArgumentNullException(nameof(processedFilesStorageDirectoryPath));
        }

        Sha256OwnerDirectoryNameProvider directoryNameProvider = new();
        LocalFileStorage<OriginalFile> originalFileStorage = new(originalFilesStorageDirectoryPath, directoryNameProvider);
        LocalFileStorage<ProcessedFile> processedFileStorage = new(processedFilesStorageDirectoryPath, directoryNameProvider);
        originalFileStorage.EnsureCreated();
        processedFileStorage.EnsureCreated();
        
        builder.Services.AddSingleton<IFileStorage<OriginalFile>>(_ => originalFileStorage);
        builder.Services.AddSingleton<IFileStorage<ProcessedFile>>(_ => processedFileStorage);
    }

    public static void ConfigureRepositories(this WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<IFileRepository<OriginalFile>, OriginalFilesRepository>();
        builder.Services.AddTransient<IFileRepository<ProcessedFile>, ProcessedFilesRepository>();
        builder.Services.AddTransient<IProcessedFileRepository, ProcessedFilesRepository>();
    }

    public static void ConfigureMqtt(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IMqttService, RabbitMqService>();
    }
    
    public static void ConfigureMediatR(this WebApplicationBuilder builder)
    {
        var assembly = typeof(SuccessfulResponses<>).Assembly;
        // var assembly = Assembly.GetExecutingAssembly();
        builder.Services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(assembly));
        builder.Services.AddMediatRAttributedBehaviors(assembly);
    }
}