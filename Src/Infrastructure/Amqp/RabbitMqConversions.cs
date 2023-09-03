using System.Diagnostics;
using Domain.AggregateModels;
using Domain.SeedWork.Interfaces;
using Domain.SeedWork.Services.Amqp;

namespace Infrastructure.Amqp;

internal static class RabbitMqConversions
{
    public static string GetRouteKey<TFile>(FileMessage<TFile> message) where TFile : UniqueEntity, IFile
        => message switch
        {
            FileUploadedMessage => nameof(FileUploadedMessage),
            DeleteOriginalFileMessage => nameof(DeleteOriginalFileMessage),
            DeleteProcessedFileMessage => nameof(DeleteProcessedFileMessage),
            FileProcessingStoppedMessage => nameof(FileProcessingStoppedMessage),
            FileProcessingFinishedMessage => nameof(FileProcessingFinishedMessage),
            _ => throw new UnreachableException($"Unexpected file message type: {message.GetType()}.")
        };
    
    public static string GetRouteKey(Type type)
        => type switch
        {
            not null when type == typeof(FileUploadedMessage) => nameof(FileUploadedMessage),
            not null when type == typeof(DeleteOriginalFileMessage) => nameof(DeleteOriginalFileMessage),
            not null when type == typeof(DeleteProcessedFileMessage) => nameof(DeleteProcessedFileMessage),
            not null when type == typeof(FileProcessingStoppedMessage) => nameof(FileProcessingStoppedMessage),
            not null when type == typeof(FileProcessingFinishedMessage) => nameof(FileProcessingFinishedMessage),
            _ => throw new UnreachableException($"Unexpected file message type: {type}.")
        };

    public static string GetQueueName(string routeKey) => $"{routeKey}_queue";

    public static string GetQueueName<TFileMessage, TFile>()
        where TFileMessage : FileMessage<TFile>
        where TFile : UniqueEntity, IFile
        => GetQueueName(GetRouteKey(typeof(TFileMessage)));
}