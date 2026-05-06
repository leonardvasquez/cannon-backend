using Microsoft.AspNetCore.Mvc;
using Cannon.Business.Services;
using Cannon.Business.DTOs;

namespace Cannon.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BoxesController : ControllerBase
{
    private readonly IBoxBL _boxBL;

    public BoxesController(IBoxBL boxBL)
    {
        _boxBL = boxBL;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var boxes = await _boxBL.GetAllActiveAsync();
        return Ok(boxes);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateBoxDto dto)
    {
        try
        {
            var box = await _boxBL.CreateAsync(dto);
            return StatusCode(201, box);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { error = ex.Message });
        }
    }

    [HttpPost("{boxId}/pack")]
    public async Task<IActionResult> Pack(int boxId, [FromBody] PackDto dto)
    {
        try
        {
            var towel = await _boxBL.PackAsync(boxId, dto);
            return Ok(towel);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}