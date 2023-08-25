using Application.Constants;
using Application.Requests.Payloads;
using FluentValidation;

namespace Application.Validators;

public class FilePaginationPayloadValidator : AbstractValidator<FilePaginationPayload>
{
    public FilePaginationPayloadValidator()
    {
        RuleFor(payload => payload.Limit)
            .GreaterThanOrEqualTo(RequestPayloadLimitations.FilePagination.LimitMinimum)
            .LessThanOrEqualTo(RequestPayloadLimitations.FilePagination.LimitMaximum);

        RuleFor(payload => payload.Offset)
            .GreaterThanOrEqualTo(RequestPayloadLimitations.FilePagination.OffsetMinimum);

        RuleFor(payload => payload.Order)
            .Matches(RequestPayloadLimitations.FilePagination.OrderRegex)
            .Must(HaveUniqueKeys);
    }

    private static bool HaveUniqueKeys(string order)
    {
        var keys = order.Split(",")
            .Select(keyValuePair => keyValuePair.Split(":")[0])
            .ToArray();

        return keys.Length == keys.Distinct().Count();
    }
}