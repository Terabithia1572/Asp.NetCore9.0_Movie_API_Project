using MovieApi.Application.Features.CQRSDesignPattern.Queries.EpisodeQueries;
using MovieApi.Application.Features.CQRSDesignPattern.Results.EpisodeResults;
using MovieApi.Persistence.Context;
using System.Threading.Tasks;

namespace MovieApi.Application.Features.CQRSDesignPattern.Handlers.EpisodeHandlers
{
    public class GetEpisodeByIDQueryHandler
    {
        private readonly MovieContext _context;

        public GetEpisodeByIDQueryHandler(MovieContext context)
        {
            _context = context;
        }

        public async Task<GetEpisodeByIDQueryResult?> Handle(GetEpisodeByIDQuery query)
        {
            var value = await _context.Episodes.FindAsync(query.EpisodeID);
            if (value == null) return null;

            return new GetEpisodeByIDQueryResult
            {
                EpisodeID = value.EpisodeID,
                SeasonID = value.SeasonID,
                EpisodeNumber = value.EpisodeNumber,
                EpisodeTitle = value.EpisodeTitle,
                Overview = value.Overview,
                DurationMinutes = value.DurationMinutes,
                AirDate = value.AirDate,
                StillImageUrl = value.StillImageUrl
            };
        }
    }
}
