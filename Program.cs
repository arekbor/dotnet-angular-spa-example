using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using SpaAngular.Data;
using SpaAngular.Models;
using SpaAngular.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
.AddCookie()
.AddGoogle(options => {
    var clientId = builder.Configuration["GoogleOAuth2ClientId"];
    if (clientId == null) {
        throw new Exception("GoogleOAuth2ClientId not found");
    }
    var clientSecret = builder.Configuration["GoogleOAuth2ClientSecret"];
    if (clientSecret == null) {
        throw new Exception("GoogleOAuth2ClientSecret not found");
    }
    options.ClientId = clientId;
    options.ClientSecret = clientSecret;
    options.SaveTokens = true;
    options.CallbackPath = new PathString("/api/oauth2/callback/google");

    //maps claims from google oauth
    options.ClaimActions.MapJsonKey("urn:google:picture", "picture", "url");

    //custom claims
    options.Events = new OAuthEvents{
        OnCreatingTicket = async ctx => {
            var userService = ctx.HttpContext.RequestServices.GetRequiredService<UserService>();
            await userService.PersistUserWhenNotExists(ctx);
        }
    };
});
builder.Services.AddAuthorization();
builder.Services.AddSpaStaticFiles(config => {
    config.RootPath = "ClientApp/dist/client-app";
});
builder.Services.AddDbContext<ApplicationDbContext>(context => {
    var connectionString = builder.Configuration["ConnectionStrings:Default"];
    if (connectionString == null) {
        throw new Exception("ConnectionStrings:Default not found");
    }
    context.UseNpgsql(connectionString);
});

builder.Services.AddScoped<UserService>();

var app = builder.Build();
app.UseDefaultFiles();
app.UseSpaStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(_ => {});
app.MapControllers();

app.UseSpa(spa => {
    spa.Options.SourcePath = "ClientApp";
    if (app.Environment.IsDevelopment()) {
        spa.UseAngularCliServer("start");
    }
});
app.Run();
