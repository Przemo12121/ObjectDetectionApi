using Application.Constants;
using Application.Requests;
using Application.Responses;
using Application.Services.MqttServices;
using Domain.AggregateModels;
using Domain.AggregateModels.OriginalFileAggregate;
using MediatR;

namespace Application.Handlers;

public class DeleteOriginalFileCommandHandler : IRequestHandler<DeleteOriginalFileCommand, IApplicationResponse>
{
    private readonly IMqttService _mqttService;
    private readonly IFileRepository<OriginalFile> _fileRepository;

    public DeleteOriginalFileCommandHandler(IMqttService mqttService, IFileRepository<OriginalFile> fileRepository)
        => (_mqttService, _fileRepository) = (mqttService, fileRepository);


    public async Task<IApplicationResponse> Handle(DeleteOriginalFileCommand request, CancellationToken cancellationToken)
    {
        await _fileRepository.RemoveAsync(request.Resource!);
        _mqttService.Enqueue(request.Resource!);
        return CreateSuccessResponse(request.Resource!);
    }
    
    private static IApplicationResponse CreateSuccessResponse(OriginalFile file)
        => new OperationSuccessfulResponse(
                ResponseMessages.Successes.FileDeleted(file.Id, typeof(OriginalFile)));
}