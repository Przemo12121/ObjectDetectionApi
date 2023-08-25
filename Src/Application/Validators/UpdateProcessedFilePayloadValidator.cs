using Application.Requests.Payloads;
using FluentValidation;

namespace Application.Validators;

public class UpdateProcessedFilePayloadValidator : AbstractValidator<UpdateProcessedFilePayload>
{
    public UpdateProcessedFilePayloadValidator()
    {
        RuleForEach(payload => payload.ViewersEmails)
            .EmailAddress();
    }
}