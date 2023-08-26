using Application.Requests;
using Application.Requests.Payloads;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("files/processed")]
public class ProcessedFilesController : BaseController
{
    private readonly IMediator _mediator;

    public ProcessedFilesController(IMediator mediator)
        =>  _mediator = mediator;

    [HttpGet, Route("{id:guid}")]
    public async Task<IActionResult> Download<T>([FromQuery] Guid id)
    {
        DownloadProcessedFileQuery request = new(id, GetRequester());
        
        var response = await _mediator.Send(request);

        return MapResponse(response);
    }
    
    [HttpGet, Route("")]
    public async Task<IActionResult> GetList<T>([FromQuery] FilePaginationPayload payload)
    {
        GetProcessedFileListQuery request = new(payload, GetRequester());
        
        var response = await _mediator.Send(request);

        return MapResponse(response);
    }
    
    [HttpDelete, Route("${id:guid}")]
    public async Task<IActionResult> Delete<T>([FromRoute] Guid id)
    {
        DeleteProcessedFileCommand request = new(id, GetRequester());
    
        var response = await _mediator.Send(request);
        
        return MapResponse(response);
    }
    
    [HttpPatch, Route("${id:guid}")]
    public async Task<IActionResult> Update<T>([FromRoute] Guid id, [FromBody] UpdateProcessedFilePayload payload)
    {
        UpdateProcessedFileCommand request = new(id, payload, GetRequester());

        var response = await _mediator.Send(request);
        
        return MapResponse(response);
    }
}