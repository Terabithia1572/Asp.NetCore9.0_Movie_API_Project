using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApi.DTOs.DTOs.UserRegisterDTOs
{
    public class CreateUserRegisterDTO
    {
        public string Name { get; set; } //Ad
        public string Surname { get; set; } //Soyad
        public string Email { get; set; } //Email
        public string Username { get; set; } //Kullanıcı Adı
        public string Password { get; set; } //Şifre
    }
}
