using Application.Requests;
using Application.Responses;
using Domain.AggregateModels.AccessAccountAggregate;
using Domain.SeedWork.Interfaces;
using MediatR;

namespace Application.PipelineBehaviors;

public class OwnerAccessVerificationPipelineBehavior<TRequest, TFile> : IPipelineBehavior<TRequest, IApplicationResponse> 
    where TRequest : IModifyCommand<TFile>
    where TFile : class, IIdentifiable<Guid>, IOwnable<AccessAccount>
{
    public async Task<IApplicationResponse> Handle(TRequest request, RequestHandlerDelegate<IApplicationResponse> next, CancellationToken cancellationToken)
        => request.Resource!.Owner.Equals(request.Requester)
            ? await next()
            : new ActionForbiddenResponse(request.Resource!.Id, typeof(TFile));
}