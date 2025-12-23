using MovieApi.Application.Features.CQRSDesignPattern.Queries.SeriesQueries;
using MovieApi.Application.Features.CQRSDesignPattern.Results.SeriesResults;
using MovieApi.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApi.Application.Features.CQRSDesignPattern.Handlers.SeriesHandlers
{
    public class GetSeriesByIDQueryHandler
    {
        private readonly MovieContext _context;

        public GetSeriesByIDQueryHandler(MovieContext context)
        {
            _context = context;
        }
        public async Task<GetSeriesByIDQueryResult> Handle(GetSeriesByIDQuery query)
        {
            var value=await _context.Series.FindAsync(query.SeriesID);
            return new GetSeriesByIDQueryResult
            {
                SeriesID = value.SeriesID,
                SeriesTitle = value.SeriesTitle,
                SeriesCoverImageURL = value.SeriesCoverImageURL,
                SeriesRating = value.SeriesRating,
                SeriesDescription = value.SeriesDescription,
                FirstAirDate = value.FirstAirDate,
                SeriesCreatedYear = value.SeriesCreatedYear,
                SeriesAverageEpisodeDuration = value.SeriesAverageEpisodeDuration,
                SeriesSeasonCount = value.SeriesSeasonCount,
                SeriesEpisodeCount = value.SeriesEpisodeCount,
                SeriesStatus = value.SeriesStatus,
                CategoryID = value.CategoryID
            };
        }
    }
}
