using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpaAngular.Services;

namespace SpaAngular.Controllers;

[Authorize]
[Route("api/oauth2")]
[ApiController]
public class AuthController : ControllerBase {

    private UserService _userService;
    public AuthController(UserService userService)
    {
        _userService = userService;
    }

    [AllowAnonymous]
    [HttpGet("authorize/google")]
    public async Task AuthorizeGoogle() {
        var properties = new AuthenticationProperties{ 
            RedirectUri = "/", 
            //if you want enable autologin remove below line
            Items = { { "prompt", "consent" } } 
        };

        await HttpContext.ChallengeAsync(properties);
    }

    [HttpPost("callback/google")]
    public IActionResult CallbackGoogle() {
        return Redirect("/");
    }

    [HttpPost("signout/google")]
    public async Task<IActionResult> Signout() {
        await HttpContext.SignOutAsync();
        return Ok();
    }
}