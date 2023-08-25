using Application.Constants;
using Application.Requests.Payloads;
using FluentValidation;

namespace Application.Validators;

public class FileStreamPayloadValidator : AbstractValidator<FileStreamPayload>
{
    public FileStreamPayloadValidator()
    {
        RuleFor(payload => payload.Stream.Length)
            .LessThanOrEqualTo(RequestPayloadLimitations.FileUpload.MaxByteSize)
            .WithMessage($"File size exceeds allowed maximum size of {RequestPayloadLimitations.FileUpload.MaxByteSize} bytes.");

        RuleFor(payload => payload.FileName)
            .NotEmpty();
        
        RuleFor(payload => payload.MimeTypes)
            .NotEmpty()
            .Must(AllBeAllowedMimeType)
            .WithMessage($"File is not any of allowed mime types: {String.Join(", ", RequestPayloadLimitations.FileUpload.AllowedMimes)}.");
    }

    private static bool AllBeAllowedMimeType(IReadOnlyCollection<string> mimeTypes)
        => mimeTypes.All(mime => RequestPayloadLimitations.FileUpload.AllowedMimes.Contains(mime));
}