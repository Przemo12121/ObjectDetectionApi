using Application.Requests;
using Application.Requests.Payloads;
using Domain.AggregateModels;
using Domain.AggregateModels.OriginalFileAggregate;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("files/original")]
public class OriginalFilesController : Controller
{
    private readonly IFileStorage<OriginalFile> _fileStorage;
    private readonly IMediator _mediator;

    public OriginalFilesController(IFileStorage<OriginalFile> fileStorage, IMediator mediator)
        => (_fileStorage, _mediator) = (fileStorage, mediator);

    [HttpPost, Route("")]
    public async Task<IActionResult> Upload([FromForm] IFormFile? file)
    {
        var response = await _mediator.Send(new DownloadProcessedFileQuery(
            Guid.NewGuid(),
            "test@owner.dev"));

        return Ok(response);
        if (file is null)
        {
            return BadRequest();
        }

        return Ok();
    }
    
    [HttpGet, Route("")]
    public IActionResult Download()
    {
        FilePath path = "616161406262622E636363/0922a091c9794300901fb9c20be74ae8";        
        var x = _fileStorage.Read(path);
        
        return x is null
            ? NotFound() 
            : Ok(x);
    }
    
    [HttpDelete, Route("")]
    public IActionResult Delete()
    {
        FilePath path = "616161406262622E636363/6b1f06fe1dba4b928b86c6f9452d2dae";        
        _fileStorage.Delete(path);
        
        return Ok();
    }
}