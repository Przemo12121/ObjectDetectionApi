using Application.PipelineBehaviors.Extensions;
using Application.Requests;
using Application.Responses;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace Application.PipelineBehaviors;

public class PayloadValidationPipelineBehavior<TRequest, TPayload> : IPipelineBehavior<TRequest, IApplicationResponse>
    where TRequest : IRequest<IApplicationResponse>, IPayloadCommand<TPayload> 
    where TPayload : class
{
    private readonly IEnumerable<IValidator<TPayload>> _validators;

    public PayloadValidationPipelineBehavior(IEnumerable<IValidator<TPayload>> validators)
        => _validators = validators;

    public async Task<IApplicationResponse> Handle(TRequest request, RequestHandlerDelegate<IApplicationResponse> next, CancellationToken cancellationToken)
    {
        (bool validationSuccessful, var failures) = Validate(request.Payload);

        return validationSuccessful
            ? await next()
            : failures.GroupByProperty().ToBadRequest();
    }

    private (bool validationSuccessful, List<ValidationFailure> failures) Validate(TPayload payload)
    {
        var errors = _validators.Select(validator => validator.Validate(payload))
            .Where(result => !result.IsValid)
            .SelectMany(result => result.Errors)
            .ToList();

        return (!errors.Any(), errors);
    }
}