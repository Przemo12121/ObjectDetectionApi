using System.Diagnostics;
using Application.Constants;
using Application.Requests;
using Application.Requests.Payloads;
using Application.Responses;
using Domain.AggregateModels;
using Domain.AggregateModels.OriginalFileAggregate;
using Domain.SeedWork.Enums;
using Domain.SeedWork.Services.Amqp;
using MediatR;

namespace Application.Handlers;

public class UploadFileCommandHandler : IRequestHandler<UploadFileCommand, IApplicationResponse>
{
    private readonly IFileRepository<OriginalFile> _fileRepository;
    private readonly IFileStorage<OriginalFile> _fileStorage;
    private readonly IAmqpService _amqpService;

    public UploadFileCommandHandler(IFileStorage<OriginalFile> fileStorage, IFileRepository<OriginalFile> fileRepository, IAmqpService amqpService)
        => (_fileStorage, _fileRepository, _amqpService) = (fileStorage, fileRepository, amqpService);

    public async Task<IApplicationResponse> Handle(UploadFileCommand request, CancellationToken cancellationToken)
    {
        var path = await _fileStorage.SaveAsync(request.Payload.Stream, request.Requester);
        
        OriginalFile entity = new(
            CreateMetadata(request.Payload),
            new(_fileStorage.StorageType, path),
            request.Requester);
        
        await _fileRepository.AddAsync(entity);
        _amqpService.Enqueue(new FileUploadedMessage(entity));
        
        return new OperationSuccessfulResponse(ResponseMessages.Successes.FileUploaded);
    }

    private static Metadata CreateMetadata(FileStreamPayload payload)
        => new(payload.FileName, payload.MimeTypes[0] switch
        {
            "image" => MediaTypes.Image,
            "video" => MediaTypes.Video,
            _ => throw new UnreachableException($"Unexpected mime type: ${payload.MimeTypes[0]}.")
        });
}