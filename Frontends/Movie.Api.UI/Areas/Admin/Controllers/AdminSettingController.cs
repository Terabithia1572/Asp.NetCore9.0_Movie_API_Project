using Microsoft.AspNetCore.Mvc;

namespace Movie.Api.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "Admin")]
    public class AdminSettingController : Controller
    {
        private static PlatformSettingsModel _currentSettings = new PlatformSettingsModel
        {
            PlatformTitle = "Movie API Cinema Portal",
            PlatformSubtitle = "En yeni filmler, diziler ve sinema incelemeleri",
            ContactEmail = "support@movieapi.com",
            ContactPhone = "+90 (212) 555 0199",
            LogoUrl = "/MovieTheme/images/logo1.png",
            DefaultLanguage = "tr-TR",
            MaintenanceMode = false
        };

        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.v1 = "Sistem Ayarları";
            ViewBag.v2 = "Ana Sayfa";
            ViewBag.v3 = "Platform Konfigürasyonu";

            return View(_currentSettings);
        }

        [HttpPost]
        public IActionResult Index(PlatformSettingsModel model)
        {
            if (model != null)
            {
                _currentSettings = model;
                ViewBag.SuccessMessage = "Platform ayarları başarıyla güncellendi.";
            }

            return View(_currentSettings);
        }
    }

    public class PlatformSettingsModel
    {
        public string PlatformTitle { get; set; } = null!;
        public string PlatformSubtitle { get; set; } = null!;
        public string ContactEmail { get; set; } = null!;
        public string ContactPhone { get; set; } = null!;
        public string LogoUrl { get; set; } = null!;
        public string DefaultLanguage { get; set; } = null!;
        public bool MaintenanceMode { get; set; }
    }
}
