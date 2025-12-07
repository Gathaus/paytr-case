using Microsoft.AspNetCore.Mvc;
using PayTR.PosSelection.Application.Models;
using PayTR.PosSelection.Application.Services;

namespace PayTR.PosSelection.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PosController : ControllerBase
{
    private readonly PosSelectionService _service;

    public PosController(PosSelectionService service)
    {
        _service = service;
    }

    [HttpPost("select")]
    public ActionResult<PosSelectionResponse> SelectPos([FromBody] PosSelectionRequest request)
    {
        var result = _service.SelectBestPos(request);
        return Ok(result);
    }
}
