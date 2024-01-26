using Application.Requests;
using Application.Requests.Payloads;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("files/original")]
public class OriginalFilesController : BaseController
{
    private readonly IMediator _mediator;

    public OriginalFilesController(IMediator mediator)
        =>  _mediator = mediator;

    // [Authorize]
    [HttpPost, Route("")]
    public async Task<IActionResult> Upload<T>([FromForm] IFormFile? file)
    {
        FileStreamPayload payload = file is not null
            ? new(file.OpenReadStream(), file.FileName)
            : new(new MemoryStream(), String.Empty);
        UploadFileCommand request = new(payload, GetRequester());
        
        var response = await _mediator.Send(request);

        return MapResponse(response);
    }
    
    // [Authorize]
    [HttpGet, Route("")]
    public async Task<IActionResult> GetList([FromQuery] FilePaginationPayload payload)
    {
        GetOriginalFileListQuery request = new(payload, GetRequester());

        var response = await _mediator.Send(request);

        return MapResponse(response);
    }
    
    // [Authorize]
    [HttpDelete, Route("${id:guid}")]
    public async Task<IActionResult> Delete<T>([FromRoute] Guid id)
    {
        DeleteOriginalFileCommand request = new(id, GetRequester());

        var response = await _mediator.Send(request);
        
        return MapResponse(response);
    }
}