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
        var (Success, Errors) = await _authService.LoginAsync(command);

        if (!Success)
        {
            return Redirect($"/login?error=1");
        }

        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

        return Redirect($"/profile/{userIdClaim}");
    }


    [HttpGet("logout")]
    public async Task<IActionResult> Logout()
    {
        await _authService.Logout();

        return Redirect("/login");
    }
}