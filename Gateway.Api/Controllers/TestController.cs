using Microsoft.AspNetCore.Authorization;

namespace Gateway.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<int>> Index()
    {
        await Task.Delay(100);
        return Ok(1);
    }
}
