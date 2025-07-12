using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApi.Persistence.Identity
{
    public class AppUser:IdentityUser
    {
        public string Name { get; set; } //kullanıcıdan alınan isim
        public string Surname { get; set; } //kullanıcıdan alınan soyisim
        public string? ImageURL { get; set; } //kullanıcıdan alınan profil resmi
    }
}
