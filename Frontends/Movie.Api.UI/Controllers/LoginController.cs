using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Movie.Api.UI.Models;
using MovieApi.Persistence.Identity;
using System.Security.Claims;

namespace Movie.Api.UI.Controllers;

public class LoginController(UserManager<AppUser> users) : Controller
{
    [HttpGet] public IActionResult Index() => RedirectToAction(nameof(SignIn));
    [HttpGet("/signin")][HttpGet("/admin/signin.html")][HttpGet("/Login/SignIn")]
    public IActionResult SignIn(string? returnUrl = null) => View("~/Views/Flix/SignIn.cshtml", new FlixLoginInput { ReturnUrl = returnUrl });

    [HttpPost("/signin")][HttpPost("/Login/SignIn")][ValidateAntiForgeryToken]
    public async Task<IActionResult> SignIn(FlixLoginInput model)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var user = await users.FindByNameAsync(model.Username) ?? await users.FindByEmailAsync(model.Username);
                if (user != null && !await users.IsLockedOutAsync(user) && await users.CheckPasswordAsync(user, model.Password))
                {
                    var claims = new List<Claim> { new(ClaimTypes.NameIdentifier, user.Id), new(ClaimTypes.Name, user.UserName ?? model.Username) };
                    claims.AddRange((await users.GetRolesAsync(user)).Select(role => new Claim(ClaimTypes.Role, role)));
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme)),
                        new AuthenticationProperties { IsPersistent = model.RememberMe, ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7) });
                    return LocalRedirect(Url.IsLocalUrl(model.ReturnUrl) ? model.ReturnUrl! : "/");
                }
                ModelState.AddModelError("", "Invalid credentials or account unavailable.");
            }
            catch (Microsoft.Data.SqlClient.SqlException) { ModelState.AddModelError("", "Account service is temporarily unavailable."); }
        }
        return View("~/Views/Flix/SignIn.cshtml", model);
    }

    [HttpPost("/signout")][HttpPost("/Login/LogOut")][ValidateAntiForgeryToken]
    public async Task<IActionResult> LogOut()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return LocalRedirect("/");
    }

    [HttpGet("/forgot")][HttpGet("/admin/forgot.html")]
    public IActionResult Forgot() => View("~/Views/Flix/Forgot.cshtml");
}
