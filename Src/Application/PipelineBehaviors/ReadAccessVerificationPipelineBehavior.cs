using Application.Requests;
using Application.Responses;
using Domain.AggregateModels.ProcessedFileAggregate;
using MediatR;

namespace Application.PipelineBehaviors;

public class ReadAccessVerificationPipelineBehavior : IPipelineBehavior<DownloadProcessedFileQuery, IApplicationResponse>
{
    public async Task<IApplicationResponse> Handle(DownloadProcessedFileQuery request, RequestHandlerDelegate<IApplicationResponse> next, CancellationToken cancellationToken)
        => request.Resource!.Owner.Equals(request.Requester) || request.Resource!.Viewers.Contains(request.Requester)
            ? await next()
            : new ActionForbiddenResponse(request.ResourceId, typeof(ProcessedFile));
}