using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.OAuth;
using SpaAngular.Data;
using SpaAngular.Models;

namespace SpaAngular.Services;

public class UserService {
    private ApplicationDbContext _applicationDbContext;

    public UserService(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    /*
    * this function is very draft. Rectify this!
    */
    public void PersistUserWhenNotExists(OAuthCreatingTicketContext context) {
        var userId = context.Identity?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) {
            throw new Exception("userId not found");
        }

        var claim = new Claim(ClaimTypes.Role, value: "User");
        context.Identity?.AddClaim(claim);

        var user = _applicationDbContext.Users.FirstOrDefault(x => x.Id == userId);
        if (user == null) {
            var newUser = new User{
                Id = context.Identity?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value,
                Name = context.Identity?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value,
                Email = context.Identity?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                Picture = context.Identity?.Claims.FirstOrDefault(x => x.Type == "urn:google:picture")?.Value,
                Role = context.Identity?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value,
            };
            _applicationDbContext.Users.Add(newUser);
            _applicationDbContext.SaveChangesAsync();
        }
    }
}