using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MovieApi.Domain.Entities;
using MovieApi.Persistence.Identity;

namespace MovieApi.Persistence.Context
{
    public class MovieContext : IdentityDbContext<AppUser>
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

            builder.Entity<UserFavorite>(entity =>
            {
                entity.ToTable("UserFavorites", "dbo");
                entity.HasKey(uf => uf.UserFavoriteID);

                entity.HasOne<AppUser>()
                    .WithMany()
                    .HasForeignKey(uf => uf.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(uf => uf.Movie)
                    .WithMany(m => m.UserFavorites)
                    .HasForeignKey(uf => uf.MovieID)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(uf => uf.Series)
                    .WithMany(s => s.UserFavorites)
                    .HasForeignKey(uf => uf.SeriesID)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<MovieCast>(entity =>
            {
                entity.ToTable("MovieCasts", "dbo");
                entity.HasKey(mc => mc.MovieCastID);

                entity.HasOne(mc => mc.Movie)
                    .WithMany(m => m.MovieCasts)
                    .HasForeignKey(mc => mc.MovieID)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(mc => mc.Cast)
                    .WithMany(c => c.MovieCasts)
                    .HasForeignKey(mc => mc.CastID)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<SeriesCast>(entity =>
            {
                entity.ToTable("SeriesCasts", "dbo");
                entity.HasKey(sc => sc.SeriesCastID);

                entity.HasOne(sc => sc.Series)
                    .WithMany(s => s.SeriesCasts)
                    .HasForeignKey(sc => sc.SeriesID)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(sc => sc.Cast)
                    .WithMany(c => c.SeriesCasts)
                    .HasForeignKey(sc => sc.CastID)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<MovieTag>(entity =>
            {
                entity.ToTable("MovieTags", "dbo");
                entity.HasKey(mt => mt.MovieTagID);

                entity.HasOne(mt => mt.Movie)
                    .WithMany(m => m.MovieTags)
                    .HasForeignKey(mt => mt.MovieID)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(mt => mt.Tag)
                    .WithMany(t => t.MovieTags)
                    .HasForeignKey(mt => mt.TagID)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<SeriesTag>(entity =>
            {
                entity.ToTable("SeriesTags", "dbo");
                entity.HasKey(st => st.SeriesTagID);

                entity.HasOne(st => st.Series)
                    .WithMany(s => s.SeriesTags)
                    .HasForeignKey(st => st.SeriesID)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(st => st.Tag)
                    .WithMany(t => t.SeriesTags)
                    .HasForeignKey(st => st.TagID)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Cast> Casts { get; set; }
        public DbSet<Series> Series { get; set; }
        public DbSet<Season> Seasons { get; set; }
        public DbSet<Episode> Episodes { get; set; }
        public DbSet<UserFavorite> UserFavorites { get; set; }
        public DbSet<MovieCast> MovieCasts { get; set; }
        public DbSet<SeriesCast> SeriesCasts { get; set; }
        public DbSet<MovieTag> MovieTags { get; set; }
        public DbSet<SeriesTag> SeriesTags { get; set; }
    }
}
