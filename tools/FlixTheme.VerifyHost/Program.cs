// Read-only browser verification host. This is never loaded by the application.
// Binds to loopback, accepts GET/HEAD only, and uses an existing user's ID solely to render views.
using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Movie.Api.UI.Controllers;
using Movie.Api.UI.Services;
using MovieApi.DTOs.DTOs.UserDTOs;
using MovieApi.Persistence.Context;
using MovieApi.Persistence.Identity;

var uiPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../../Frontends/Movie.Api.UI"));
var builder = WebApplication.CreateBuilder(new WebApplicationOptions { Args = args, ContentRootPath = uiPath, WebRootPath = Path.Combine(uiPath, "wwwroot"), EnvironmentName = "Development" });
builder.WebHost.UseUrls("http://127.0.0.1:5019");
builder.Services.AddControllersWithViews().AddApplicationPart(typeof(FlixController).Assembly);
builder.Services.AddHttpClient();
builder.Services.AddHttpClient("MovieApi", client => { client.BaseAddress = new Uri("http://localhost:5114/api/"); client.Timeout = TimeSpan.FromSeconds(15); });
builder.Services.AddScoped<MovieApiClient>();
builder.Services.AddScoped<FlixCatalogService>();
builder.Services.AddDbContext<MovieContext>();
builder.Services.AddIdentityCore<AppUser>().AddRoles<IdentityRole>().AddEntityFrameworkStores<MovieContext>();
builder.Services.AddAuthentication("ReadOnlyVerification").AddScheme<AuthenticationSchemeOptions, VerificationAuthentication>("ReadOnlyVerification", _ => { });
var app = builder.Build();
app.Use(async (context, next) =>
{
    if (context.Request.Method is not ("GET" or "HEAD")) { context.Response.StatusCode = 405; return; }
    // Legacy deletion endpoints are GETs; never expose them from this verification host.
    if (context.Request.Path.Value?.Contains("Delete", StringComparison.OrdinalIgnoreCase) == true
        || context.Request.Path.Value?.Contains("Remove", StringComparison.OrdinalIgnoreCase) == true) { context.Response.StatusCode = 405; return; }
    await next();
});
app.UseStaticFiles();
app.UseStatusCodePagesWithReExecute("/Error/NotFound404");
app.UseRouting();
app.Use(async (context, next) =>
{
    var action = context.GetEndpoint()?.Metadata.GetMetadata<Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor>();
    if (action != null && action.ControllerName is not ("Flix" or "FlixAdmin" or "Profile" or "Login" or "Register" or "Error"))
    { context.Response.StatusCode = 405; return; }
    await next();
});
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute("areas", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute("default", "{controller=Flix}/{action=Index}/{id?}");
app.Run();

sealed class VerificationAuthentication(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, MovieApiClient api)
    : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var user = (await api.ListAsync<ResultUserDto>("Users")).FirstOrDefault();
        if (user == null) return AuthenticateResult.NoResult();
        var identity = new ClaimsIdentity([new Claim(ClaimTypes.NameIdentifier, user.Id), new Claim(ClaimTypes.Name, user.UserName), new Claim(ClaimTypes.Role, "Admin")], Scheme.Name);
        return AuthenticateResult.Success(new AuthenticationTicket(new ClaimsPrincipal(identity), Scheme.Name));
    }
}
