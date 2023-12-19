using System.Diagnostics;
using Domain.AggregateModels;
using Domain.AggregateModels.OriginalFileAggregate;
using Domain.AggregateModels.ProcessedFileAggregate;

namespace AiService.ProcessingHandlers.PythonScriptsHandlers;

public class PythonAiProcessingHandler : IProcessingHandler
{
    private readonly IFileStorage<OriginalFile> _originalFilesStorage;
    private readonly IFileStorage<ProcessedFile> _processedFilesStorage;
    private readonly string _pythonScriptPath;
    private readonly string _pythonExecutablePath;
    private readonly Dictionary<OriginalFile, Process> _filesCancellationTokenLookupTable = new();
    
    public PythonAiProcessingHandler(
        string pythonExecutablePath,
        string pythonScriptPath,
        IFileStorage<OriginalFile> originalFileStorage, 
        IFileStorage<ProcessedFile> processedFilesStorage) 
        => (_pythonExecutablePath, _pythonScriptPath, _originalFilesStorage, _processedFilesStorage) 
            = (pythonExecutablePath, pythonScriptPath, originalFileStorage, processedFilesStorage);

    public void BeginProcessing(OriginalFile file)
    {
        if (_filesCancellationTokenLookupTable.ContainsKey(file))
        {
            // throw
        }
        

        // ProcessStartInfo info = new();
        // info.FileName = _pythonExecutablePath;
        // //todo create processed file obj
        // info.Arguments = $"{_pythonScriptPath} ${file.StorageData.Uri} ${"TODO"}";
        // info.CreateNoWindow = false;
        // var process = Process.Start(info)!; // TODO handle null
        var process = Process.Start(_pythonExecutablePath, new[] { _pythonScriptPath, file.StorageData.Uri });
        
        _filesCancellationTokenLookupTable.Add(file, process);
    }
    
    public void StopProcessing(OriginalFile file)
    {
        if (!_filesCancellationTokenLookupTable.ContainsKey(file))
        {
            return;
        }
        
        _filesCancellationTokenLookupTable[file].Kill();
        _filesCancellationTokenLookupTable.Remove(file);
        // clean processedFile associated file here (create in begin, add to lookup)
    }
}