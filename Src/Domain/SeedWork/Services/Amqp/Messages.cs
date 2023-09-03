using Domain.AggregateModels;
using Domain.AggregateModels.OriginalFileAggregate;
using Domain.AggregateModels.ProcessedFileAggregate;
using Domain.SeedWork.Interfaces;

namespace Domain.SeedWork.Services.Amqp;

public abstract record FileMessage<T>(T File) where T : UniqueEntity, IFile;

public record FileUploadedMessage(OriginalFile File) : FileMessage<OriginalFile>(File);

public record DeleteOriginalFileMessage(OriginalFile File) : FileMessage<OriginalFile>(File);

public record DeleteProcessedFileMessage(ProcessedFile File) : FileMessage<ProcessedFile>(File);

public record FileProcessingStoppedMessage(OriginalFile File) : FileMessage<OriginalFile>(File);

public record FileProcessingFinishedMessage(OriginalFile File) : FileMessage<OriginalFile>(File);
