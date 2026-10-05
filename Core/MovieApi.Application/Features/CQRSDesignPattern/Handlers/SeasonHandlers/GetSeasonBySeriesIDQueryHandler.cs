using Microsoft.EntityFrameworkCore;
using MovieApi.Application.Features.CQRSDesignPattern.Queries.SeasonQueries;
using MovieApi.Application.Features.CQRSDesignPattern.Results.SeasonResults;
using MovieApi.Persistence.Context;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApi.Application.Features.CQRSDesignPattern.Handlers.SeasonHandlers
{
    public class GetSeasonBySeriesIDQueryHandler
    {
        private readonly MovieContext _context;

        public GetSeasonBySeriesIDQueryHandler(MovieContext context)
        {
            _context = context;
        }

        public async Task<List<GetSeasonBySeriesIDQueryResult>> Handle(GetSeasonBySeriesIDQuery query)
        {
            var seriesExists = await _context.Series.AnyAsync(s => s.SeriesID == query.SeriesID);
            if (!seriesExists)
            {
                throw new KeyNotFoundException($"Series with ID {query.SeriesID} was not found.");
            }

            var values = await _context.Seasons
                .Where(x => x.SeriesID == query.SeriesID)
                .ToListAsync();

            return values.Select(x => new GetSeasonBySeriesIDQueryResult
            {
                SeasonID = x.SeasonID,
                SeriesID = x.SeriesID,
                SeasonNumber = x.SeasonNumber,
                Overview = x.Overview,
                AirDate = x.AirDate,
                PosterImageUrl = x.PosterImageUrl
            }).ToList();
        }
    }
}
