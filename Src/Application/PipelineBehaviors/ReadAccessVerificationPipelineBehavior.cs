using Application.Requests;
using Application.Responses;
using Domain.AggregateModels.AccessAccountAggregate;
using Domain.AggregateModels.ProcessedFileAggregate;
using Domain.SeedWork.Interfaces;
using MediatR;

namespace Application.PipelineBehaviors;

public class ReadAccessVerificationPipelineBehavior : IPipelineBehavior<DownloadProcessedFileQuery, IApplicationResponse> 
{
    public async Task<IApplicationResponse> Handle(DownloadProcessedFileQuery request, RequestHandlerDelegate<IApplicationResponse> next, CancellationToken cancellationToken)
    {
        if (request.Resource!.Owner.Equals(request.Requester)
            || request.Resource!.Viewers.Contains(request.Requester))
        {
            return await next();
        }

        return new ActionForbidden(request.ResourceId, typeof(ProcessedFile));
    }
    
}