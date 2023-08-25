using Application.Constants;
using Application.Requests;
using Application.Responses;
using Application.Services.MqttServices;
using Domain.AggregateModels;
using Domain.AggregateModels.OriginalFileAggregate;
using Domain.AggregateModels.ProcessedFileAggregate;
using MediatR;

namespace Application.Handlers;

public class UpdateProcessedFileCommandHandler : IRequestHandler<UpdateProcessedFileCommand, IApplicationResponse>
{
    private readonly IProcessedFileRepository _fileRepository;

    public UpdateProcessedFileCommandHandler(IProcessedFileRepository fileRepository)
        => (_fileRepository) = fileRepository;


    public async Task<IApplicationResponse> Handle(UpdateProcessedFileCommand request, CancellationToken cancellationToken)
    {
        await _fileRepository.UpdateAsync(request.Resource!);
        return CreateSuccessResponse(request.Resource!);
    }
    
    private static IApplicationResponse CreateSuccessResponse(ProcessedFile file)
        => new OperationSuccessfulResponse(
                ResponseMessages.Successes.FileUpdated(file.Id, typeof(OriginalFile)));
}