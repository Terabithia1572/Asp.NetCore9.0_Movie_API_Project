using MovieApi.Application.Features.CQRSDesignPattern.Queries.SeasonQueries;
using MovieApi.Application.Features.CQRSDesignPattern.Results.SeasonResults;
using MovieApi.Persistence.Context;
using System.Threading.Tasks;

namespace MovieApi.Application.Features.CQRSDesignPattern.Handlers.SeasonHandlers
{
    public class GetSeasonByIDQueryHandler
    {
        private readonly MovieContext _context;

        public GetSeasonByIDQueryHandler(MovieContext context)
        {
            _context = context;
        }

        public async Task<GetSeasonByIDQueryResult?> Handle(GetSeasonByIDQuery query)
        {
            var value = await _context.Seasons.FindAsync(query.SeasonID);
            if (value == null) return null;

            return new GetSeasonByIDQueryResult
            {
                SeasonID = value.SeasonID,
                SeriesID = value.SeriesID,
                SeasonNumber = value.SeasonNumber,
                Overview = value.Overview,
                AirDate = value.AirDate,
                PosterImageUrl = value.PosterImageUrl
            };
        }
    }
}
