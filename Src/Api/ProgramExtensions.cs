using Application.Requests.Payloads;
using Application.Responses;
using Application.Validators;
using Domain.AggregateModels;
using Domain.AggregateModels.OriginalFileAggregate;
using Domain.AggregateModels.ProcessedFileAggregate;
using Domain.SeedWork.Services.Amqp;
using FluentValidation;
using Infrastructure.Amqp;
using Infrastructure.Database;
using Infrastructure.FileStorage;
using Infrastructure.FileStorage.OwnerDirectoryNameProviders;
using Infrastructure.Repositories;
using MediatR.Extensions.AttributedBehaviors;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;

namespace Api;

public static class ProgramExtensions
{
    public static void ConfigureDatabase(this WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<DbContextOptions<ObjectDetectionDbContext>>(
            _ => new DbContextOptionsBuilder<ObjectDetectionDbContext>()
                .UseNpgsql(builder.Configuration.GetConnectionString("ObjectDetectionDb"), b => b.MigrationsAssembly("Api"))
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

    public static void ConfigureAmqp(this WebApplicationBuilder builder)
    {
        var rabbitMqSection = builder.Configuration.GetSection("Amqp").GetSection("RabbitMQ");
        
        builder.Services.AddSingleton<IAmqpService, RabbitMq>(_ => RabbitMq.Connect(
            new ConnectionFactory
            {
                UserName = rabbitMqSection.GetValue<string>("Username"),
                Password = rabbitMqSection.GetValue<string>("Password"),
                Port = rabbitMqSection.GetValue<int>("Port"),
                ClientProvidedName = rabbitMqSection.GetValue<string>("Password")
            }));
    }
    
    public static void ConfigureMediatR(this WebApplicationBuilder builder)
    {
        var assembly = typeof(SuccessfulResponse<>).Assembly;
        builder.Services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(assembly));
        builder.Services.AddMediatRAttributedBehaviors(assembly);
    }

    public static void ConfigureValidators(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IValidator<FilePaginationPayload>, FilePaginationPayloadValidator>();
        builder.Services.AddSingleton<IValidator<FileStreamPayload>, FileStreamPayloadValidator>();
        builder.Services.AddSingleton<IValidator<UpdateProcessedFilePayload>, UpdateProcessedFilePayloadValidator>();
    }
    
    public static void ConfigureOAuth(this WebApplicationBuilder builder)
    {
        var googleOAuthSection = builder.Configuration.GetSection("Authentication").GetSection("Google");

        builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
                options.DefaultSignOutScheme = IdentityConstants.ExternalScheme;
            })
            .AddCookie(options =>
            {
                options.Events.OnRedirectToLogin = (ctx) =>
                {
                    ctx.Response.StatusCode = 401;
                    return Task.CompletedTask;
                };
            })
            .AddGoogle(googleOptions =>
            {
                googleOptions.Scope.Add("email");
                // googleOptions.CallbackPath = "/signin-google"; // registered in google console
                googleOptions.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                googleOptions.ClientId = googleOAuthSection.GetValue<string>("ClientId")!;
                googleOptions.ClientSecret = googleOAuthSection.GetValue<string>("ClientSecret")!;
            });
    }
}