using System.Text;
using Newtonsoft.Json;
using Domain.AggregateModels;
using Domain.SeedWork.Interfaces;
using Domain.SeedWork.Services.Amqp;
using Infrastructure.Amqp.Converters;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Infrastructure.Amqp;

public sealed class RabbitMq : IAmqpService, IDisposable
{
    private IConnection Connection { get; }
    private IModel Channel { get; }
    private IBasicProperties PublishProperties { get; }

    private const string Exchange = "object_detection_amqp_main_exchange";
    private readonly AccessAccountConverter _accessAccountConverter = new();

    private RabbitMq(IConnectionFactory factory)
    {
        Connection = factory.CreateConnection();
        Channel = Connection.CreateModel();
        PublishProperties = Channel.CreateBasicProperties();
        PublishProperties.Persistent = true;
    }
    
    public static RabbitMq Connect(IConnectionFactory factory)
    {
        RabbitMq instance = new(factory);
        instance.Channel.ExchangeDeclare(Exchange, ExchangeType.Direct);

        Type[] types =
        {
            typeof(FileUploadedMessage),
            typeof(DeleteOriginalFileMessage),
            typeof(DeleteProcessedFileMessage),
            typeof(FileProcessingStoppedMessage),
            typeof(FileProcessingFinishedMessage)
        };

        string queue;
        string routeKey;
        foreach (var type in types)
        {
            routeKey = RabbitMqConversions.GetRouteKey(type);
            queue = RabbitMqConversions.GetQueueName(routeKey);
            instance.Channel.QueueDeclare(queue, true, false, false);
            instance.Channel.QueueBind(queue, Exchange, routeKey);
        }
        
        return instance;
    }

    public void CreateConsumer<TFileMessage, TFile>(string consumerTag, Action<TFileMessage> action) 
        where TFileMessage : FileMessage<TFile>
        where TFile : UniqueEntity, IFile
    {
        EventingBasicConsumer consumer = new(Channel);
        
        consumer.Received += (_, args) =>
        {
            var msgString = Encoding.UTF8.GetString(args.Body.ToArray());
            var message = JsonConvert.DeserializeObject<TFileMessage>(msgString, _accessAccountConverter);
            
            if (message is null)
            {
                throw new Exception($"Unexpected error while deserializing message: {msgString}.");
            }

            action(message);
        };
        
        Channel.BasicConsume(
            queue: RabbitMqConversions.GetQueueName<TFileMessage, TFile>(), 
            consumer: consumer,
            consumerTag: consumerTag, 
            autoAck: true);
    }
    
    public void Enqueue<T>(FileMessage<T> message) where T : UniqueEntity, IFile
    {
        var routeKey = RabbitMqConversions.GetRouteKey(message);
        var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
        Channel.BasicPublish(Exchange, routeKey, PublishProperties, body);
    }

    public void Dispose()
    {
        Channel.Close();
        Connection.Close();
        
        Channel.Dispose();
        Connection.Dispose();
    }
}