using Domain.AggregateModels;
using Domain.AggregateModels.OriginalFileAggregate;
using Domain.AggregateModels.ProcessedFileAggregate;
using Domain.SeedWork.Services.Amqp;
using Infrastructure.Amqp;
using Infrastructure.FileStorage;
using Infrastructure.FileStorage.OwnerDirectoryNameProviders;
using RabbitMQ.Client;

if (args.Length < 3)
{
    throw new ArgumentException("Insufficient amount of cli arguments.");
}

var originalFilesDirectory = args[0];
var processedFilesDirectory = args[1];
var rabbitConnectionString = args[2]!
    .Split(";")
    .Select(keyValuePair => keyValuePair.Split("="))
    .ToDictionary(keyValuePair => keyValuePair[0], keyValuePair => keyValuePair[1]);

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

amqpService.CreateConsumer<FileProcessingStoppedMessage, OriginalFile>(
    "OnProcessingStopCleanup", 
    message => originalFilesStorage.Delete(message.File.StorageData.Uri));
amqpService.CreateConsumer<FileProcessingFinishedMessage, OriginalFile>(
    "OnProcessingFinishCleanup", 
    message => originalFilesStorage.Delete(message.File.StorageData.Uri));
amqpService.CreateConsumer<DeleteProcessedFileMessage, ProcessedFile>(
    "OnProcessedFileDeletionCleanup", 
    message => processedFilesStorage.Delete(message.File.StorageData.Uri));

Console.CancelKeyPress += (_, _) => Console.WriteLine("Cleanup service shutting down.\n");
Console.WriteLine("Cleanup service running.");

while(true); // run program until shutdown