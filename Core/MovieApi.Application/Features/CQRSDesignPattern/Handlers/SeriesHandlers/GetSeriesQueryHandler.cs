using Microsoft.EntityFrameworkCore;
using MovieApi.Application.Features.CQRSDesignPattern.Results.SeriesResults;
using MovieApi.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApi.Application.Features.CQRSDesignPattern.Handlers.SeriesHandlers
{
    public class GetSeriesQueryHandler
    {
        private readonly MovieContext _context;

        public GetSeriesQueryHandler(MovieContext context)
        {
            _context = context;
        }
        public async Task<List<GetSeriesQueryResult>> Handle()
        {
            var values=await _context.Series.ToListAsync();
            return values.Select(x=>new GetSeriesQueryResult
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
                SeriesEpisodeCount = x.SeriesEpisodeCount,
                SeriesStatus = x.SeriesStatus,
                CategoryID = x.CategoryID
            }).ToList();
        }
    }
}
