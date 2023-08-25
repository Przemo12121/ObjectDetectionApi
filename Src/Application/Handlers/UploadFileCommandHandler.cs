using System.Diagnostics;
using Application.Constants;
using Application.Requests;
using Application.Requests.Payloads;
using Application.Responses;
using Domain.AggregateModels;
using Domain.AggregateModels.OriginalFileAggregate;
using Domain.SeedWork.Enums;
using MediatR;

namespace Application.Handlers;

public class UploadFileCommandHandler : IRequestHandler<UploadFileCommand, IApplicationResponse>
{
    private readonly IFileRepository<OriginalFile> _fileRepository;
    private readonly IFileStorage<OriginalFile> _fileStorage;

    public UploadFileCommandHandler(IFileStorage<OriginalFile> fileStorage, IFileRepository<OriginalFile> fileRepository)
        => (_fileStorage, _fileRepository) = (fileStorage, fileRepository);

    public async Task<IApplicationResponse> Handle(UploadFileCommand request, CancellationToken cancellationToken)
    {
        var path = await _fileStorage.SaveAsync(request.Payload.Stream, request.Requester);

        await _fileRepository.AddAsync(new(
            CreateMetadata(request.Payload),
            new(_fileStorage.StorageType, path),
            request.Requester));

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