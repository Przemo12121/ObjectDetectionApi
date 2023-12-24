using System.Diagnostics;
using Domain.AggregateModels;
using Domain.AggregateModels.AccessAccountAggregate;
using Domain.AggregateModels.OriginalFileAggregate;
using Domain.AggregateModels.ProcessedFileAggregate;
using Domain.SeedWork.Enums;

namespace AiService.ProcessingHandlers.PythonScriptsHandlers;

public class PythonAiProcessingHandler : IProcessingHandler
{
    private readonly IFileStorage<OriginalFile> _originalFilesStorage;
    private readonly IFileStorage<ProcessedFile> _processedFilesStorage;
    private readonly Dictionary<OriginalFile, CancellationTokenSource> _filesCancellationTokenSourcesLookUpTable = new();
    
    public PythonAiProcessingHandler(IFileStorage<OriginalFile> originalFileStorage, IFileStorage<ProcessedFile> processedFilesStorage) 
        => (_originalFilesStorage, _processedFilesStorage) = (originalFileStorage, processedFilesStorage);

    public void BeginProcessing(OriginalFile file)
    {
        Console.WriteLine($"Process begun for file with id: {file.Id}");
        lock (_filesCancellationTokenSourcesLookUpTable)
        {
            if (_filesCancellationTokenSourcesLookUpTable.ContainsKey(file))
            {
                throw new Exception($"Original file with id: {file.Id} is already being processed.");
            }

            CancellationTokenSource tokenSource = new();
            _filesCancellationTokenSourcesLookUpTable.Add(file, tokenSource);

            var _ = new Thread(() => HandleProcessingInBackgroundThread(file, tokenSource));
        }
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
    }

    private async void HandleProcessingInBackgroundThread(OriginalFile file, CancellationTokenSource tokenSource)
    {
        // creates empty file to write processed data to
        var newFilePath = await _processedFilesStorage.SaveAsync(new MemoryStream(), file.Owner);
        
        ProcessedFile newFile = new(
            file.Owner,
            file.Metadata,
            new(file.StorageData.StorageType, newFilePath),
            new(""), // TODO: serving service -> uri to get method with guid
            Array.Empty<AccessAccount>());
        
        var process = RunProcess(file.Metadata.Type, file.StorageData.Uri, newFile.StorageData.Uri);

        while (!process.HasExited)
        {
            if (!tokenSource.IsCancellationRequested) continue;
            
            process.Kill();
            _processedFilesStorage.Delete(newFile.StorageData.Uri);
        }
        
        process.Dispose();
        // insert to db

        lock (_filesCancellationTokenSourcesLookUpTable)
        {
            _filesCancellationTokenSourcesLookUpTable.Remove(file);
        }
    }
    
    private static Process RunProcess(MediaTypes mediaType, string inputFile, string outputFile)
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