using System.Diagnostics;
using Domain.AggregateModels;
using Domain.AggregateModels.AccessAccountAggregate;
using Domain.AggregateModels.OriginalFileAggregate;
using Domain.AggregateModels.ProcessedFileAggregate;
using Domain.SeedWork.Enums;
using Domain.SeedWork.Services.Amqp;
using Infrastructure.FileServers;
using Infrastructure.Repositories;
using Infrastructure.Repositories.Factories;

namespace AiService.ProcessingHandlers.PythonScriptsHandlers;

public class PythonAiProcessingHandler : IProcessingHandler
{
    private readonly IFileStorage<OriginalFile> _originalFilesStorage;
    private readonly IFileStorage<ProcessedFile> _processedFilesStorage;
    private readonly IRepositoryFactory<ProcessedFilesRepository, ProcessedFile> _repositoryFactory;
    private readonly Dictionary<OriginalFile, CancellationTokenSource> _filesCancellationTokenSourcesLookUpTable = new();
    private readonly IAmqpService _amqpService;
    private readonly IFileServerUrlFactory _urlFactory;

    public PythonAiProcessingHandler(
        IFileStorage<OriginalFile> originalFileStorage, 
        IFileStorage<ProcessedFile> processedFilesStorage, 
        IRepositoryFactory<ProcessedFilesRepository, ProcessedFile> repositoryFactory, 
        IAmqpService amqpService, 
        IFileServerUrlFactory urlFactory)
    {
        _repositoryFactory = repositoryFactory;
        _amqpService = amqpService;
        _urlFactory = urlFactory;
        _originalFilesStorage = originalFileStorage;
        _processedFilesStorage = processedFilesStorage;
    }

    public void BeginProcessing(OriginalFile file)
    {
        Console.WriteLine($"Process begun for file with id: {file.Id}");
        CancellationTokenSource tokenSource = new();

        lock (_filesCancellationTokenSourcesLookUpTable)
        {
            if (_filesCancellationTokenSourcesLookUpTable.ContainsKey(file))
            {
                throw new Exception($"Original file with id: {file.Id} is already being processed.");
            }

            _filesCancellationTokenSourcesLookUpTable.Add(file, tokenSource);
        }

        Thread thread = new(async () => await HandleProcessingInBackgroundThread(file, tokenSource));
        thread.Start();
    }
    
    public void StopProcessing(OriginalFile file)
    {
        Console.WriteLine($"Process being stopped for file with id: {file.Id}");

        lock (_filesCancellationTokenSourcesLookUpTable)
        {
            if (!_filesCancellationTokenSourcesLookUpTable.ContainsKey(file))
            {
                return;
            }

            _filesCancellationTokenSourcesLookUpTable[file].Cancel();
            _filesCancellationTokenSourcesLookUpTable.Remove(file);
        }
        
        _amqpService.Enqueue(new FileProcessingStoppedMessage(file));
    }

    private async Task HandleProcessingInBackgroundThread(OriginalFile file, CancellationTokenSource tokenSource)
    {
        // creates empty file to write processed data to
        var newFilePath = await _processedFilesStorage.SaveAsync(new MemoryStream(), file.Owner);
        ProcessedFile newFile = new(
            file.Owner,
            file.Metadata,
            file.StorageData with { Uri = newFilePath},
            new ServeData(""),
            Array.Empty<AccessAccount>());

        newFile.ServeData = new(_urlFactory.Create(newFile));

        var newFileFullPath = _processedFilesStorage.GetFullPath(newFile);
        var originalFileFullPath = _originalFilesStorage.GetFullPath(file);
        var process = CreateAndRunProcess(file.Metadata.Type, originalFileFullPath, newFileFullPath);

        while (!process.HasExited)
        {
            if (!tokenSource.IsCancellationRequested) continue;
            
            process.Kill();
            _processedFilesStorage.Delete(newFile.StorageData.Uri);
        }

        process.Dispose();
        
        lock (_filesCancellationTokenSourcesLookUpTable)
        {
            _filesCancellationTokenSourcesLookUpTable.Remove(file);
        }
        
        _amqpService.Enqueue(new FileProcessingFinishedMessage(file));
        await _repositoryFactory.Create()
            .AddAsync(newFile);
    }

    private static Process CreateAndRunProcess(MediaTypes mediaType, string inputFile, string outputFile)
    {
        var media = mediaType == MediaTypes.Image ? "image" : "video";
        Process process = new();
        process.StartInfo.Arguments = $"Python/main.py {media} {inputFile} {outputFile} ./Python/Models/D0 0.5";
        process.StartInfo.FileName = "python";
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.RedirectStandardOutput = true;
        process.OutputDataReceived += (_, _) => { };
         
        process.Start();

        return process;
    }
}