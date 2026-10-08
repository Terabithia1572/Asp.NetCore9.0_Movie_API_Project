using Microsoft.AspNetCore.Mvc;
namespace Movie.Api.UI.Controllers;
public class ContactController : Controller
{
    public IActionResult Index() => RedirectToAction("Contacts", "Flix");
    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult SendMessage() => StatusCode(501, "Message delivery is not configured. No message was sent.");
}
