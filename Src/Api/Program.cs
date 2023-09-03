using System.Diagnostics;
using Api;
using Domain.AggregateModels.AccessAccountAggregate;
using Domain.AggregateModels.OriginalFileAggregate;
using Domain.AggregateModels.ProcessedFileAggregate;
using Domain.SeedWork.Enums;
using Domain.SeedWork.Services.Amqp;
using Infrastructure.Amqp;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.ConfigureDatabase();
builder.ConfigureRepositories();
builder.ConfigureFileStorages();
builder.ConfigureMediatR();
builder.ConfigureMqtt();
builder.ConfigureValidators();

var x = RabbitMq.Connect(new ConnectionFactory()
{
    UserName = "object_detection_amqp",
    Password = "GLkWl3v0fl3y2CW7cX7Z",
    Port = 5672,
    ClientProvidedName = "testapp"
});

x.Enqueue(new DeleteProcessedFileMessage(
    new ProcessedFile(
        "xd@wtf.lol", 
        new("123", MediaTypes.Image), 
        new(FileStorageTypes.LocalStorage, "s"), 
        new("ttt"),
        new List<AccessAccount>() { "a@b.c", "c@b.d" })));

x.Enqueue(new FileUploadedMessage(
    new OriginalFile(
        new("123", MediaTypes.Image), 
        new(FileStorageTypes.LocalStorage, "s"), 
        "123@bc.d")));

x.Enqueue(new DeleteProcessedFileMessage(
    new ProcessedFile(
        "456@bc.d", 
        new("123456", MediaTypes.Image), 
        new(FileStorageTypes.LocalStorage, "s"), 
        new("ttt"),
        new List<AccessAccount>() { })));

x.Enqueue(new DeleteProcessedFileMessage(
    new ProcessedFile(
        "6789@bc.d", 
        new("123456788", MediaTypes.Image), 
        new(FileStorageTypes.LocalStorage, "s"), 
        new("ttt"),
        new List<AccessAccount>() { "aaa@bbb.ccc", "dddd@eeee.fff" })));

Action<DeleteProcessedFileMessage> act = message =>
{
    Console.Write($"new message (outer scope): {message.File.Metadata.Name} & owner: {message.File.Owner}\t");
    Console.WriteLine($"new message (outer scope): <X> {String.Join(", ", message.File.Viewers)}");
};

// Console.ReadLine();
// Thread.Sleep(300);
// x.CreateConsumer<DeleteProcessedFileMessage, ProcessedFile>("testcons", act);
x.CreateConsumer<DeleteProcessedFileMessage, ProcessedFile>("testcons", act);
Console.ReadLine();
return;
var app = builder.Build();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

