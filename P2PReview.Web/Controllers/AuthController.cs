using Microsoft.AspNetCore.Mvc;
using P2PReview.Application.Auth.Commands;
using P2PReview.Application.Interfaces;
using System.Security.Claims;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromForm] LoginCommand command)
    {
        var response = await _authService.LoginAsync(command);

        if (!response.Success)
        {
            return Redirect($"login?error={response.Errors}");
        }

        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

        return Redirect($"/profile/{userIdClaim}");
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterCommand command)
    {
        var response = await _authService.RegisterAsync(command);

        if (!response.Success)
        {
            return BadRequest(response.Errors);
        }

        return Ok();
    }
}