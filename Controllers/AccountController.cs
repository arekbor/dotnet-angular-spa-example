using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpaAngular.Dtos;

namespace SpaAngular.Controllers;

[Authorize]
[Route("api/account")]
[ApiController]
public class AccountController : ControllerBase {
    [HttpGet]
    public IActionResult Get() {
        var userDto = new UserDto
        {
            Email = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
            Name = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value,
            Picture = User.Claims.FirstOrDefault(x => x.Type == "urn:google:picture")?.Value,
            Role = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value
        };
        return Ok(userDto);
    }
} 