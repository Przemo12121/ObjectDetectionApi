using Application.Responses;
using Application.Responses.Payloads;
using FluentValidation.Results;

namespace Application.PipelineBehaviors.Extensions;

public static class PayloadValidationPipelineBehaviorExtensions
{
    public static Dictionary<string, List<string>> GroupByProperty(this IEnumerable<ValidationFailure> failures)
        => failures.GroupBy(failure => failure.PropertyName)
            .ToDictionary(
                grouped => grouped.First().PropertyName,
                grouped => grouped.Select(g => g.ErrorMessage).ToList());

    public static BadRequestResponse ToBadRequest(this Dictionary<string, List<string>> groupedFailures)
        => new(new ErrorListPayload { ErrorList = groupedFailures });
}