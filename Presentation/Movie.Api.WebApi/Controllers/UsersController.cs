using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieApi.Persistence.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Movie.Api.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;

        public UsersController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            var userList = users.Select(u => new
            {
                u.Id,
                u.UserName,
                u.Email,
                u.EmailConfirmed,
                u.PhoneNumber,
                u.Name,
                u.Surname,
                IsLockedOut = u.LockoutEnd.HasValue && u.LockoutEnd.Value > DateTimeOffset.UtcNow,
                LockoutEnd = u.LockoutEnd
            }).ToList();

            return Ok(userList);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound("Kullanıcı bulunamadı.");
            }

            return Ok(new
            {
                user.Id,
                user.UserName,
                user.Email,
                user.EmailConfirmed,
                user.PhoneNumber,
                user.Name,
                user.Surname,
                IsLockedOut = user.LockoutEnd.HasValue && user.LockoutEnd.Value > DateTimeOffset.UtcNow,
                LockoutEnd = user.LockoutEnd
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UserUpdateModel model)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound("Kullanıcı bulunamadı.");
            }

            if (!string.IsNullOrEmpty(model.Name)) user.Name = model.Name;
            if (!string.IsNullOrEmpty(model.Surname)) user.Surname = model.Surname;
            if (!string.IsNullOrEmpty(model.Email)) user.Email = model.Email;
            if (!string.IsNullOrEmpty(model.PhoneNumber)) user.PhoneNumber = model.PhoneNumber;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return Ok("Kullanıcı başarıyla güncellendi.");
            }

            return BadRequest(result.Errors);
        }

        [HttpPost("{id}/toggle-status")]
        public async Task<IActionResult> ToggleUserStatus(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound("Kullanıcı bulunamadı.");
            }

            bool isCurrentlyLocked = user.LockoutEnd.HasValue && user.LockoutEnd.Value > DateTimeOffset.UtcNow;
            if (isCurrentlyLocked)
            {
                user.LockoutEnd = null;
            }
            else
            {
                user.LockoutEnd = DateTimeOffset.UtcNow.AddYears(100);
            }

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return Ok(new
                {
                    isLockedOut = !isCurrentlyLocked,
                    message = !isCurrentlyLocked ? "Kullanıcı engellendi." : "Kullanıcı engeli kaldırıldı."
                });
            }

            return BadRequest("Kullanıcı durumu değiştirilemedi.");
        }
    }

    public class UserUpdateModel
    {
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
