using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieApi.Persistence.Context;
using System.Linq;
using System.Threading.Tasks;

namespace Movie.Api.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly MovieContext _context;

        public DashboardController(MovieContext context)
        {
            _context = context;
        }

        [HttpGet("stats")]
        public async Task<IActionResult> GetDashboardStats()
        {
            var totalMovies = await _context.Movies.CountAsync();
            var totalSeries = await _context.Series.CountAsync();
            var totalCategories = await _context.Categories.CountAsync();
            var totalCasts = await _context.Casts.CountAsync();
            var totalUsers = await _context.Users.CountAsync();
            var totalReviews = await _context.Reviews.CountAsync();

            var topRatedMovies = await _context.Movies
                .OrderByDescending(m => m.MovieRating)
                .Take(5)
                .Select(m => new
                {
                    m.MovieID,
                    m.MovieTitle,
                    m.MovieCoverImageURL,
                    m.MovieRating,
                    m.MovileCreatedYear
                })
                .ToListAsync();

            var topRatedSeries = await _context.Series
                .OrderByDescending(s => s.SeriesRating)
                .Take(5)
                .Select(s => new
                {
                    s.SeriesID,
                    s.SeriesTitle,
                    s.SeriesCoverImageURL,
                    s.SeriesRating,
                    s.SeriesCreatedYear
                })
                .ToListAsync();

            var latestReviews = await _context.Reviews
                .OrderByDescending(r => r.ReviewDate)
                .Take(5)
                .Select(r => new
                {
                    r.ReviewID,
                    UserComment = r.ReviewComment,
                    r.UserRating,
                    r.ReviewDate,
                    r.MovieID,
                    MovieTitle = r.Movie != null ? r.Movie.MovieTitle : "N/A"
                })
                .ToListAsync();

            return Ok(new
            {
                totalMovies,
                totalSeries,
                totalCategories,
                totalCasts,
                totalUsers,
                totalReviews,
                topRatedMovies,
                topRatedSeries,
                latestReviews
            });
        }
    }
}
