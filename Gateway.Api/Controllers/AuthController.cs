namespace Gateway.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ITokenService _tokenService;

    public AuthController(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        // 🔐 Replace with actual DB/auth validation
        if (request.Username == "admin" && request.Password == "admin123")
        {
            var token = _tokenService.GenerateToken(request.Username, "Admin", "tenant-abc");
            return Ok(new { token });
        }

        return Unauthorized();
    }
}

public record LoginRequest(string Username, string Password);