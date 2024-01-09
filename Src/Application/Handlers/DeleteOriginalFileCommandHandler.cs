using Application.Constants;
using Application.Requests;
using Application.Responses;
using Domain.AggregateModels;
using Domain.AggregateModels.OriginalFileAggregate;
using Domain.SeedWork.Services.Amqp;
using MediatR;

namespace Application.Handlers;

public class DeleteOriginalFileCommandHandler : IRequestHandler<DeleteOriginalFileCommand, IApplicationResponse>
{
    private readonly IAmqpService _amqpService;
    private readonly IFileRepository<OriginalFile> _fileRepository;

    public DeleteOriginalFileCommandHandler(IAmqpService amqpService, IFileRepository<OriginalFile> fileRepository)
        => (_amqpService, _fileRepository) = (amqpService, fileRepository);

    public async Task<IApplicationResponse> Handle(DeleteOriginalFileCommand request, CancellationToken cancellationToken)
    {
        await _fileRepository.RemoveAsync(request.Resource!);
        _amqpService.Enqueue(new DeleteOriginalFileMessage(request.Resource!));
        return CreateSuccessResponse(request.Resource!);
    }
    
    private static IApplicationResponse CreateSuccessResponse(OriginalFile file)
        => new OperationSuccessfulResponse(
                ResponseMessages.Successes.FileDeleted(file.Id, typeof(OriginalFile)));
}