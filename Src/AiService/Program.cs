using AiService.ProcessingHandlers;
using AiService.ProcessingHandlers.PythonScriptsHandlers;
using Domain.AggregateModels;
using Domain.AggregateModels.OriginalFileAggregate;
using Domain.AggregateModels.ProcessedFileAggregate;
using Domain.SeedWork.Services.Amqp;
using Infrastructure.Amqp;
using Infrastructure.FileStorage;
using Infrastructure.FileStorage.OwnerDirectoryNameProviders;
using RabbitMQ.Client;

// TODO: move args to .env during containerization
var originalFilesDirectory = args.Length > 0 ? args[0] : "/home/przemo/Repositories/Przemo12121/ObjectDetectionApi/FILES/ORIGINAL";
var processedFilesDirectory = args.Length > 1 ? args[1] : "/home/przemo/Repositories/Przemo12121/ObjectDetectionApi/FILES/PROCESSED";
var rabbitUsername = args.Length > 2 ? args[2] : "object_detection_amqp";
var rabbitPassword = args.Length > 3 ? args[3] : "GLkWl3v0fl3y2CW7cX7Z";
var rabbitPort = args.Length > 4 ? args[4] : "5672";
var rabbitClientProvidedName = args.Length > 5 ? args[5] : "AiService";

IFileStorage<OriginalFile> originalFilesStorage = new LocalFileStorage<OriginalFile>(originalFilesDirectory, new Sha256OwnerDirectoryNameProvider()); 
IFileStorage<ProcessedFile> processedFilesStorage = new LocalFileStorage<ProcessedFile>(processedFilesDirectory, new Sha256OwnerDirectoryNameProvider()); 
IAmqpService amqpService = RabbitMq.Connect(new ConnectionFactory
{
    UserName = rabbitUsername,
    Password = rabbitPassword,
    Port = Convert.ToInt32(rabbitPort),
    ClientProvidedName = rabbitClientProvidedName
});

IProcessingHandler handler = new PythonAiProcessingHandler(originalFilesStorage, processedFilesStorage);

amqpService.CreateConsumer<DeleteOriginalFileMessage, OriginalFile>(
    "OnOriginalFileDeletion_AiService", 
    message => handler.StopProcessing(message.File));
amqpService.CreateConsumer<FileUploadedMessage, OriginalFile>(
    "OnFileUploaded_AiService", 
    message => handler.BeginProcessing(message.File));

Console.CancelKeyPress += (_, _) => Console.WriteLine("Ai service shutting down.");
Console.WriteLine("Ai service running.");

while(true); // run program until shutdown