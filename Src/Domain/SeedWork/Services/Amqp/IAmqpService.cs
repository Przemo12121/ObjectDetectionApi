using Domain.AggregateModels;
using Domain.SeedWork.Interfaces;

namespace Domain.SeedWork.Services.Amqp;

public interface IAmqpService
{
    void Enqueue<T>(FileMessage<T> message) where T : UniqueEntity, IFile;

    void CreateConsumer<TFileMessage, TFile>(string consumerTag, Action<TFileMessage> action)
        where TFileMessage : FileMessage<TFile>
        where TFile : UniqueEntity, IFile;
}