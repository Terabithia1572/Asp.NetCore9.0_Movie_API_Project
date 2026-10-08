using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Movie.Api.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "Admin")]
    public class AdminLogController : Controller
    {
        public IActionResult LogList()
        {
            ViewBag.v1 = "Sistem Logları";
            ViewBag.v2 = "Ana Sayfa";
            ViewBag.v3 = "Sistem & Denetim Logları";

            var logs = new List<SystemLogItem>
            {
                new SystemLogItem { LogId = 1, LogLevel = "Information", Message = "Kullanıcı başarıyla giriş yaptı.", Source = "AuthService", Timestamp = DateTime.Now.AddMinutes(-5), RequestPath = "/Login/SignIn" },
                new SystemLogItem { LogId = 2, LogLevel = "Information", Message = "Yeni film eklendi: Esaretin Bedeli", Source = "MovieController", Timestamp = DateTime.Now.AddMinutes(-18), RequestPath = "/api/Movies" },
                new SystemLogItem { LogId = 3, LogLevel = "Warning", Message = "Başarısız oturum açma denemesi (admin@test.com)", Source = "AuthService", Timestamp = DateTime.Now.AddMinutes(-42), RequestPath = "/Login/SignIn" },
                new SystemLogItem { LogId = 4, LogLevel = "Information", Message = "Favori film eklendi.", Source = "UserFavoritesController", Timestamp = DateTime.Now.AddHours(-2), RequestPath = "/api/UserFavorites/toggle-movie" },
                new SystemLogItem { LogId = 5, LogLevel = "Error", Message = "Önbellek sağlayıcısına erişilemedi, varsayılan DB kullanılıyor.", Source = "CacheService", Timestamp = DateTime.Now.AddHours(-5), RequestPath = "/api/Series" },
                new SystemLogItem { LogId = 6, LogLevel = "Information", Message = "Sezon güncellemesi yapıldı (Sezon #1)", Source = "SeasonController", Timestamp = DateTime.Now.AddHours(-8), RequestPath = "/api/seasons" }
            };

            return View(logs);
        }
    }

    public class SystemLogItem
    {
        public int LogId { get; set; }
        public string LogLevel { get; set; } = null!;
        public string Message { get; set; } = null!;
        public string Source { get; set; } = null!;
        public DateTime Timestamp { get; set; }
        public string RequestPath { get; set; } = null!;
    }
}
