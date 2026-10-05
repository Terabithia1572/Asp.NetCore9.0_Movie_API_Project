using Microsoft.EntityFrameworkCore;
using MovieApi.Application.Features.CQRSDesignPattern.Results.EpisodeResults;
using MovieApi.Persistence.Context;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApi.Application.Features.CQRSDesignPattern.Handlers.EpisodeHandlers
{
    public class GetEpisodeQueryHandler
    {
        private readonly MovieContext _context;

        public GetEpisodeQueryHandler(MovieContext context)
        {
            _context = context;
        }

        public async Task<List<GetEpisodeQueryResult>> Handle()
        {
            var values = await _context.Episodes.ToListAsync();

            return values.Select(x => new GetEpisodeQueryResult
            {
                EpisodeID = x.EpisodeID,
                SeasonID = x.SeasonID,
                EpisodeNumber = x.EpisodeNumber,
                EpisodeTitle = x.EpisodeTitle,
                Overview = x.Overview,
                DurationMinutes = x.DurationMinutes,
                AirDate = x.AirDate,
                StillImageUrl = x.StillImageUrl
            }).ToList();
        }
    }
}
