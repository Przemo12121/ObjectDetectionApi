using System.Diagnostics;
using Application.Responses;
using Domain.AggregateModels.AccessAccountAggregate;
using Domain.AggregateModels.OriginalFileAggregate;
using Domain.AggregateModels.ProcessedFileAggregate;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public abstract class BaseController : Controller
{
    protected AccessAccount GetRequester() => "todo@email.dev"; // TODO auth & OAuth

    protected IActionResult MapResponse(IApplicationResponse response)
        => response.Success
            ? MapSuccessfulResponse(response)
            : MapUnsuccessfulResponse(response);

    private IActionResult MapSuccessfulResponse(IApplicationResponse response)
        => response switch
        {
            FileListResponse<OriginalFile> r => Ok(r.Payload),
            FileListResponse<ProcessedFile> r => Ok(r.Payload),
            OperationSuccessfulResponse r => Ok(r.Payload),
            DownloadFileResponse r => Ok(r.Payload),
            _ => throw new UnreachableException($"Unexpected type of SuccessfulResponse: {response.GetType()}")
        };

    private IActionResult MapUnsuccessfulResponse(IApplicationResponse response)
        => response switch
        {
            FileNotFoundResponse r => NotFound(r.Payload),
            ActionForbiddenResponse r => Forbidden(r.Payload),
            BadRequestResponse r => BadRequest(r.Payload),
            _ => throw new UnreachableException($"Unexpected type of UnsuccessfulResponse: {response.GetType()}")
        };

    private static ObjectResult Forbidden(object payload)
        => new(payload) { StatusCode = StatusCodes.Status403Forbidden };
}