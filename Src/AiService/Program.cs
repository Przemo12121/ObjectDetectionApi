using System.Diagnostics;
// using AiService.ProcessingHandlers;
// using AiService.ProcessingHandlers.PythonScriptsHandlers;
// using Domain.AggregateModels;
// using Domain.AggregateModels.OriginalFileAggregate;
// using Domain.AggregateModels.ProcessedFileAggregate;
// using Domain.SeedWork.Services.Amqp;
// using Infrastructure.Amqp;
// using Infrastructure.FileStorage;
// using Infrastructure.FileStorage.OwnerDirectoryNameProviders;
// using Microsoft.VisualBasic;
// using RabbitMQ.Client;

// // TODO: move args to .env during containerization
// var originalFilesDirectory = args.Length > 0 ? args[0] : "/home/przemo/Repositories/Przemo12121/ObjectDetectionApi/FILES/ORIGINAL";
// var processedFilesDirectory = args.Length > 1 ? args[1] : "/home/przemo/Repositories/Przemo12121/ObjectDetectionApi/FILES/PROCESSED";
// var rabbitUsername = args.Length > 2 ? args[2] : "object_detection_amqp";
// var rabbitPassword = args.Length > 3 ? args[3] : "GLkWl3v0fl3y2CW7cX7Z";
// var rabbitPort = args.Length > 4 ? args[4] : "5672";
// var rabbitClientProvidedName = args.Length > 5 ? args[5] : "CleanupService";
// var pythonScriptPath = args.Length > 6 ? args[6] : "TODO";
// var pythonExecutablePath = args.Length > 8 ? args[8] : "TODO";
//
// IFileStorage<OriginalFile> originalFilesStorage = new LocalFileStorage<OriginalFile>(originalFilesDirectory, new Sha256OwnerDirectoryNameProvider()); 
// IFileStorage<ProcessedFile> processedFilesStorage = new LocalFileStorage<ProcessedFile>(processedFilesDirectory, new Sha256OwnerDirectoryNameProvider()); 
// IAmqpService amqpService = RabbitMq.Connect(new ConnectionFactory
// {
//     UserName = rabbitUsername,
//     Password = rabbitPassword,
//     Port = Convert.ToInt32(rabbitPort),
//     ClientProvidedName = rabbitClientProvidedName
// });
//
// IProcessingHandler handler = new PythonAiProcessingHandler(
//     pythonExecutablePath, pythonScriptPath, originalFilesStorage, processedFilesStorage);
//
// amqpService.CreateConsumer<DeleteOriginalFileMessage, OriginalFile>(
//     "OnOriginalFileDeletion_AiService", 
//     message => handler.StopProcessing(message.File));
// amqpService.CreateConsumer<FileUploadedMessage, OriginalFile>(
//     "OnFileUploaded_AiService", 
//     message => handler.BeginProcessing(message.File));
//
// Console.CancelKeyPress += (_, _) => Console.WriteLine("Ai service shutting down.");
// Console.WriteLine("Ai service running.");
//
// while(true); // run program until shutdown

using var process = new Process();
process.StartInfo.Arguments = "Python/main.py image test.jpg test_o.jpg";
process.StartInfo.FileName = "python";
// process.StartInfo.Environment.Add("TF_CPP_MIN_LOG_LEVEL", "3");
process.StartInfo.UseShellExecute = true;
// process.StartInfo.RedirectStandardOutput = true;
// process.OutputDataReceived += (_, _) => {};
process.Start();
// process.OutputDataReceived += (_, _) => { }; 
Console.WriteLine("Process started");
await process.WaitForExitAsync(default);
Console.WriteLine("Process ended");
