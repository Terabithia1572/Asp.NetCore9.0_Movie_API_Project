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
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Review>()
                .HasOne<AppUser>()          // Navigation yok!
                .WithMany()
                .HasForeignKey(r => r.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Season>(entity =>
            {
                entity.ToTable("Seasons", "dbo");
                entity.HasKey(s => s.SeasonID);

                entity.Property(s => s.Overview)
                    .HasMaxLength(2000);

                entity.Property(s => s.PosterImageUrl)
                    .HasMaxLength(1000);

                entity.HasOne(s => s.Series)
                    .WithMany(series => series.Seasons)
                    .HasForeignKey(s => s.SeriesID)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<Episode>(entity =>
            {
                entity.ToTable("Episodes", "dbo");
                entity.HasKey(e => e.EpisodeID);

                entity.Property(e => e.EpisodeTitle)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.Overview)
                    .HasMaxLength(2000);

                entity.Property(e => e.StillImageUrl)
                    .HasMaxLength(1000);

                entity.HasOne(e => e.Season)
                    .WithMany(season => season.Episodes)
                    .HasForeignKey(e => e.SeasonID)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
        public DbSet<Category> Categories { get; set; } //Kategoriler tablosu
        public DbSet<Movie> Movies { get; set; } //Filmler tablosu
        public DbSet<Review> Reviews { get; set; } //Yorumlar tablosu
        public DbSet<Tag> Tags { get; set; } //Etiketler tablosu
        public DbSet<Cast> Casts { get; set; } //Oyuncular tablosu
        public DbSet<Series> Series { get; set; } //Diziler tablosu
        public DbSet<Season> Seasons { get; set; }
        public DbSet<Episode> Episodes { get; set; }
    }
}
