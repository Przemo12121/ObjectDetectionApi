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

if (args.Length < 4) {
    throw new ArgumentException("Insufficient amount of cli arguments.");
}

var originalFilesDirectory = args[0];
var processedFilesDirectory = args[1];
var dbConnectionString = args[2]; //TODO
var rabbitConnectionString = args[3]!
    .Split(";")
    .Select(keyValuePair => keyValuePair.Split("="))
    .ToDictionary(keyValuePair => keyValuePair[0], keyValuePair => keyValuePair[1]);

foreach (var arg in args)
{
    Console.WriteLine(arg);
}

var rabbitUsername = rabbitConnectionString["Username"] ?? throw new ArgumentNullException("Rabbit 'Username' not provided.");
var rabbitPassword = rabbitConnectionString["Password"] ?? throw new ArgumentNullException("Rabbit 'Password' not provided.");
var rabbitPort = rabbitConnectionString["Port"] ?? throw new ArgumentNullException("Rabbit 'Port' not provided.");
var rabbitHost = rabbitConnectionString["Host"] ?? throw new ArgumentNullException("Rabbit 'Host' not provided.");
var rabbitClientProvidedName = rabbitConnectionString["ProvidedName"] ?? throw new ArgumentNullException("Rabbit 'ProvidedName' not provided.");

IFileStorage<OriginalFile> originalFilesStorage = new LocalFileStorage<OriginalFile>(originalFilesDirectory, new Sha256OwnerDirectoryNameProvider()); 
IFileStorage<ProcessedFile> processedFilesStorage = new LocalFileStorage<ProcessedFile>(processedFilesDirectory, new Sha256OwnerDirectoryNameProvider()); 
IAmqpService amqpService = RabbitMq.Connect(new ConnectionFactory
{
    HostName = rabbitHost,
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