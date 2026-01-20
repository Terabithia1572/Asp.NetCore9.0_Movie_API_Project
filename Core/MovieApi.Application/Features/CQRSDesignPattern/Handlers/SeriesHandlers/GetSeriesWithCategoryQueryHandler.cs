using Microsoft.EntityFrameworkCore;
using MovieApi.Application.Features.CQRSDesignPattern.Results.MovieResults;
using MovieApi.Application.Features.CQRSDesignPattern.Results.SeriesResults;
using MovieApi.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApi.Application.Features.CQRSDesignPattern.Handlers.SeriesHandlers
{
    public class GetSeriesWithCategoryQueryHandler
    {
        private readonly MovieContext _context;
        public GetSeriesWithCategoryQueryHandler(MovieContext context)
        {
            _context = context;
        }

        public async Task<List<GetSeriesWithCategoryQueryResult>> Handle()
        {
            var values = await _context.Series.Include(x => x.Category).ToListAsync();
            return values.Select(x => new GetSeriesWithCategoryQueryResult
            {
                SeriesID = x.SeriesID,
                SeriesTitle = x.SeriesTitle,
                SeriesCoverImageURL = x.SeriesCoverImageURL,
                SeriesRating = x.SeriesRating,
                SeriesDescription = x.SeriesDescription,
                FirstAirDate = x.FirstAirDate,
                SeriesCreatedYear = x.SeriesCreatedYear,
                SeriesAverageEpisodeDuration = x.SeriesAverageEpisodeDuration,
                SeriesSeasonCount = x.SeriesSeasonCount,
                SeriesStatus = x.SeriesStatus,
                CategoryID = x.Category.CategoryID,
                CategoryName = x.Category.CategoryName
            }).ToList();
        }
    }
}
