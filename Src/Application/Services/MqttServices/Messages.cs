using Domain.AggregateModels;
using Domain.SeedWork.Interfaces;

namespace Application.Services.MqttServices;

public record FileMessage<T>(T File) where T : UniqueEntity, IFile;