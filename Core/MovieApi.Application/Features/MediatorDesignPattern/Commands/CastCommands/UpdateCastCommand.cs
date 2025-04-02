using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApi.Application.Features.MediatorDesignPattern.Commands.CastCommands
{
    public class UpdateCastCommand:IRequest
    {
        //public int CastID { get; set; } //Oyuncu ID
        public string CastTitle { get; set; } //Oyuncu Başlık
        public string CastName { get; set; } //Oyuncu Ad
        public string CastSurname { get; set; } //Oyuncu Soyad
        public string CastImageURL { get; set; } //Oyuncu Resim URL
        public string? CastOverview { get; set; } //Oyuncu Genel bakış
        public string? CastBiography { get; set; } //Oyuncu Biyografisi
    }
}
