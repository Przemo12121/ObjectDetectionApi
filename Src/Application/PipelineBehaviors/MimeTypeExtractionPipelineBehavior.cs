using Application.Requests;
using Application.Responses;
using MediatR;
using MimeDetective;
using MimeDetective.Definitions;

namespace Application.PipelineBehaviors;

public class MimeTypeExtractionPipelineBehavior : IPipelineBehavior<UploadFileCommand, IApplicationResponse>
{
    private readonly ContentInspector _inspector = new ContentInspectorBuilder
    {
        Definitions = Default.All()
    }.Build();
    
    public async Task<IApplicationResponse> Handle(UploadFileCommand request, RequestHandlerDelegate<IApplicationResponse> next, CancellationToken cancellationToken)
    {
        request.Payload.MimeTypes = _inspector.Inspect(request.Payload.Stream)
            .Select(result => (result.Definition.File.MimeType ?? String.Empty).Split("/")[0].ToLower())
            .ToList();

        return await next();
    }
}