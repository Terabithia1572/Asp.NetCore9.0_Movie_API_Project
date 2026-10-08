using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Movie.Api.UI.Models;
using MovieApi.Persistence.Identity;

namespace Movie.Api.UI.Controllers;

public class RegisterController(UserManager<AppUser> users) : Controller
{
    [HttpGet] public IActionResult Index() => RedirectToAction(nameof(SignUp));
    [HttpGet("/signup")][HttpGet("/admin/signup.html")][HttpGet("/Register/SignUp")]
    public IActionResult SignUp() => View("~/Views/Flix/SignUp.cshtml", new FlixRegisterInput());

    [HttpPost("/signup")][HttpPost("/Register/SignUp")][ValidateAntiForgeryToken]
    public async Task<IActionResult> SignUp(FlixRegisterInput model)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var result = await users.CreateAsync(new AppUser { Name = model.Name, Surname = model.Surname, UserName = model.Username, Email = model.Email }, model.Password);
                if (result.Succeeded) return RedirectToAction("SignIn", "Login");
                foreach (var error in result.Errors) ModelState.AddModelError("", error.Description);
            }
            catch (Microsoft.Data.SqlClient.SqlException) { ModelState.AddModelError("", "Account service is temporarily unavailable."); }
        }
        return View("~/Views/Flix/SignUp.cshtml", model);
    }
}
