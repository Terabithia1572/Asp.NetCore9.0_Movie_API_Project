using Microsoft.EntityFrameworkCore;
using MovieApi.Application.Features.CQRSDesignPattern.Queries.EpisodeQueries;
using MovieApi.Application.Features.CQRSDesignPattern.Results.EpisodeResults;
using MovieApi.Persistence.Context;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApi.Application.Features.CQRSDesignPattern.Handlers.EpisodeHandlers
{
    public class GetEpisodeBySeasonIDQueryHandler
    {
        private readonly MovieContext _context;

        public GetEpisodeBySeasonIDQueryHandler(MovieContext context)
        {
            _context = context;
        }

        public async Task<List<GetEpisodeBySeasonIDQueryResult>> Handle(GetEpisodeBySeasonIDQuery query)
        {
            var values = await _context.Episodes
                .Where(x => x.SeasonID == query.SeasonID)
                .ToListAsync();

            return values.Select(x => new GetEpisodeBySeasonIDQueryResult
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
