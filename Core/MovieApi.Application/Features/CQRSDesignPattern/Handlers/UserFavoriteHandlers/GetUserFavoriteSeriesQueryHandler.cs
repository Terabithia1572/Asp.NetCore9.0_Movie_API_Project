using Microsoft.EntityFrameworkCore;
using MovieApi.Application.Features.CQRSDesignPattern.Queries.UserFavoriteQueries;
using MovieApi.Application.Features.CQRSDesignPattern.Results.UserFavoriteResults;
using MovieApi.Persistence.Context;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApi.Application.Features.CQRSDesignPattern.Handlers.UserFavoriteHandlers
{
    public class GetUserFavoriteSeriesQueryHandler
    {
        private readonly MovieContext _context;

        public GetUserFavoriteSeriesQueryHandler(MovieContext context)
        {
            _context = context;
        }

        public async Task<List<GetUserFavoriteSeriesQueryResult>> Handle(GetUserFavoriteSeriesQuery query)
        {
            var favorites = await _context.UserFavorites
                .Include(uf => uf.Series)
                .Where(uf => uf.UserId == query.UserId && uf.SeriesID != null)
                .ToListAsync();

            return favorites
                .Where(uf => uf.Series != null)
                .Select(uf => new GetUserFavoriteSeriesQueryResult
                {
                    UserFavoriteID = uf.UserFavoriteID,
                    UserId = uf.UserId,
                    SeriesID = uf.SeriesID!.Value,
                    CreatedDate = uf.CreatedDate,
                    SeriesTitle = uf.Series!.SeriesTitle,
                    SeriesCoverImageURL = uf.Series.SeriesCoverImageURL,
                    SeriesRating = uf.Series.SeriesRating,
                    SeriesDescription = uf.Series.SeriesDescription,
                    FirstAirDate = uf.Series.FirstAirDate,
                    SeriesCreatedYear = uf.Series.SeriesCreatedYear,
                    SeriesAverageEpisodeDuration = uf.Series.SeriesAverageEpisodeDuration,
                    SeriesSeasonCount = uf.Series.SeriesSeasonCount,
                    SeriesEpisodeCount = uf.Series.SeriesEpisodeCount,
                    CategoryID = uf.Series.CategoryID
                }).ToList();
        }
    }
}
