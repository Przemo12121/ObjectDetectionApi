using Application.Constants;
using Application.Requests;
using Application.Responses;
using Application.Services.MqttServices;
using Domain.AggregateModels;
using Domain.AggregateModels.ProcessedFileAggregate;
using MediatR;

namespace Application.Handlers;

public class DeleteProcessedFileCommandHandler : IRequestHandler<DeleteProcessedFileCommand, IApplicationResponse>
{
    private readonly IMqttService _mqttService;
    private readonly IFileRepository<ProcessedFile> _fileRepository;

    public DeleteProcessedFileCommandHandler(IMqttService mqttService, IFileRepository<ProcessedFile> fileRepository)
        => (_mqttService, _fileRepository) = (mqttService, fileRepository);


    public async Task<IApplicationResponse> Handle(DeleteProcessedFileCommand request, CancellationToken cancellationToken)
    {
        await _fileRepository.RemoveAsync(request.Resource!);
        _mqttService.Enqueue(request.Resource!);
        return CreateSuccessResponse(request.Resource!);
    }
    
    private static IApplicationResponse CreateSuccessResponse(ProcessedFile file)
        => new OperationSuccessfulResponse(
                ResponseMessages.Successes.FileDeleted(file.Id, typeof(ProcessedFile)));
}