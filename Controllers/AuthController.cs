using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SpaAngular.Controllers;

[Authorize]
[Route("api/oauth2")]
[ApiController]
public class AuthController : ControllerBase {

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