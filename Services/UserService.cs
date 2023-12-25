using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.EntityFrameworkCore;
using SpaAngular.Data;
using SpaAngular.Models;

namespace SpaAngular.Services;

public class UserService {
    private ApplicationDbContext _applicationDbContext;

    public UserService(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async Task PersistUserWhenNotExistsAsync(OAuthCreatingTicketContext context) {
        var userId = context.Identity?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) {
            throw new Exception("userId not found");
        }
        var user = await _applicationDbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);
        if (user == null) {
            var claim = new Claim(ClaimTypes.Role, value: "User");
            context.Identity?.AddClaim(claim);

            var newUser = new User{
                Id = context.Identity?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value,
                Role = context.Identity?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value,
            };
            await _applicationDbContext.Users.AddAsync(newUser);
            await _applicationDbContext.SaveChangesAsync();
        }
    }
    public async Task LoadUserFromStoreAsync(OAuthCreatingTicketContext context) {
        var userId = context.Identity?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) {
            throw new Exception("userId not found");
        }

        var user = await _applicationDbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);
        if (user == null) {
            return;
        }

        if (user.Role == null) {
            throw new Exception("user role not found");
        }
        var claim = new Claim(ClaimTypes.Role, value: user.Role);
        context.Identity?.AddClaim(claim);
    }
}