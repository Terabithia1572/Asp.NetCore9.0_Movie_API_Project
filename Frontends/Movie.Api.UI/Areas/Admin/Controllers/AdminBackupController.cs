using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace Movie.Api.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "Admin")]
    public class AdminBackupController : Controller
    {
        private static List<BackupHistoryItem> _backups = new List<BackupHistoryItem>
        {
            new BackupHistoryItem { BackupName = "ApiMovieDB_Full_20261005.bak", FileSize = "142.5 MB", BackupType = "Tam Veritabanı (Full)", CreatedDate = DateTime.Now.AddDays(-1), Status = "Başarılı" },
            new BackupHistoryItem { BackupName = "ApiMovieDB_Full_20260928.bak", FileSize = "138.2 MB", BackupType = "Tam Veritabanı (Full)", CreatedDate = DateTime.Now.AddDays(-8), Status = "Başarılı" },
            new BackupHistoryItem { BackupName = "Media_Assets_Backup_20261001.zip", FileSize = "450.0 MB", BackupType = "Medya & Görseller", CreatedDate = DateTime.Now.AddDays(-5), Status = "Başarılı" }
        };

        public IActionResult Index()
        {
            ViewBag.v1 = "Sistem Yedekleme";
            ViewBag.v2 = "Ana Sayfa";
            ViewBag.v3 = "Veritabanı & Dosya Yedekleri";

            return View(_backups);
        }

        [HttpPost]
        public IActionResult CreateBackup(string backupType)
        {
            string name = (backupType == "media" ? "Media_Assets_Backup_" : "ApiMovieDB_Full_") + DateTime.Now.ToString("yyyyMMdd_HHmmss") + (backupType == "media" ? ".zip" : ".bak");
            string size = backupType == "media" ? "465.2 MB" : "145.8 MB";
            string typeLabel = backupType == "media" ? "Medya & Görseller" : "Tam Veritabanı (Full)";

            _backups.Insert(0, new BackupHistoryItem
            {
                BackupName = name,
                FileSize = size,
                BackupType = typeLabel,
                CreatedDate = DateTime.Now,
                Status = "Başarılı"
            });

            TempData["SuccessMessage"] = $"Yeni {typeLabel} yedeği başarıyla oluşturuldu: {name}";
            return RedirectToAction("Index");
        }

        public IActionResult DownloadBackup(string fileName)
        {
            string content = $"Movie API Backup File: {fileName}\nCreated At: {DateTime.Now}\nStatus: Verified";
            return File(Encoding.UTF8.GetBytes(content), "application/octet-stream", fileName ?? "backup.bak");
        }
    }

    public class BackupHistoryItem
    {
        public string BackupName { get; set; } = null!;
        public string FileSize { get; set; } = null!;
        public string BackupType { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
        public string Status { get; set; } = null!;
    }
}
