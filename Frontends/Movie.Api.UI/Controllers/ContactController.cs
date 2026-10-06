using Microsoft.AspNetCore.Mvc;
using Movie.Api.UI.Models;

namespace Movie.Api.UI.Controllers
{
    public class ContactController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.v1 = "İletişim";
            ViewBag.v2 = "Ana Sayfa";
            ViewBag.v3 = "İletişim";
            return View();
        }

        [HttpPost]
        public IActionResult SendMessage(ContactFormModel model)
        {
            if (ModelState.IsValid)
            {
                TempData["SuccessMessage"] = "Mesajınız başarıyla iletildi. En kısa sürede sizinle iletişime geçeceğiz.";
            }
            else
            {
                TempData["ErrorMessage"] = "Lütfen tüm zorunlu alanları eksiksiz doldurunuz.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
