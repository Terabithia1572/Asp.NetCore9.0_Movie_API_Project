using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MovieApi.Domain.Entities;
using MovieApi.Persistence.Identity;

namespace MovieApi.Persistence.Context
{
    public class MovieContext:IdentityDbContext<AppUser>
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
           optionsBuilder.UseSqlServer("Server=.;Initial Catalog=ApiMovieDB;Integrated Security=True;TrustServerCertificate=true;");
        }
        public DbSet<Category> Categories { get; set; } //Kategoriler tablosu
        public DbSet<Movie> Movies { get; set; } //Filmler tablosu
        public DbSet<Review> Reviews { get; set; } //Yorumlar tablosu
        public DbSet<Tag> Tags { get; set; } //Etiketler tablosu
        public DbSet<Cast> Casts { get; set; } //Oyuncular tablosu
        public DbSet<Series> Series { get; set; } //Diziler tablosu
    }
}
