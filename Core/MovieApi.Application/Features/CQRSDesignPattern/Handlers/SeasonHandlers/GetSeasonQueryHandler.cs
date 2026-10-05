using Microsoft.EntityFrameworkCore;
using MovieApi.Application.Features.CQRSDesignPattern.Results.SeasonResults;
using MovieApi.Persistence.Context;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApi.Application.Features.CQRSDesignPattern.Handlers.SeasonHandlers
{
    public class GetSeasonQueryHandler
    {
        private readonly MovieContext _context;

        public GetSeasonQueryHandler(MovieContext context)
        {
            _context = context;
        }

        public async Task<List<GetSeasonQueryResult>> Handle()
        {
            var values = await _context.Seasons.ToListAsync();

            return values.Select(x => new GetSeasonQueryResult
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
