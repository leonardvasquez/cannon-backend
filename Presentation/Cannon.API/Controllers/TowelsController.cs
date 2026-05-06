using Microsoft.AspNetCore.Mvc;
using Cannon.Business.Services;

namespace Cannon.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TowelsController : ControllerBase
{
    private readonly ITowelBL _towelBL;

    public TowelsController(ITowelBL towelBL)
    {
        _towelBL = towelBL;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var towels = await _towelBL.GetAllActiveAsync();
        return Ok(towels);
    }
}