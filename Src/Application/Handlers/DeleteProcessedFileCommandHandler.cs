using Application.Constants;
using Application.Requests;
using Application.Responses;
using Domain.AggregateModels;
using Domain.AggregateModels.ProcessedFileAggregate;
using Domain.SeedWork.Services.Amqp;
using MediatR;

namespace Application.Handlers;

public class DeleteProcessedFileCommandHandler : IRequestHandler<DeleteProcessedFileCommand, IApplicationResponse>
{
    private readonly IAmqpService _amqpService;
    private readonly IFileRepository<ProcessedFile> _fileRepository;

    public DeleteProcessedFileCommandHandler(IAmqpService amqpService, IFileRepository<ProcessedFile> fileRepository)
        => (_amqpService, _fileRepository) = (amqpService, fileRepository);

    public async Task<IApplicationResponse> Handle(DeleteProcessedFileCommand request, CancellationToken cancellationToken)
    {
        await _fileRepository.RemoveAsync(request.Resource!);
        _amqpService.Enqueue(new DeleteProcessedFileMessage(request.Resource!));
        return CreateSuccessResponse(request.Resource!);
    }
    
    private static IApplicationResponse CreateSuccessResponse(ProcessedFile file)
        => new OperationSuccessfulResponse(
                ResponseMessages.Successes.FileDeleted(file.Id, typeof(ProcessedFile)));
}