using MovieApi.Application.Features.CQRSDesignPattern.Commands.SeasonCommands;
using MovieApi.Domain.Entities;
using MovieApi.Persistence.Context;
using System.Threading.Tasks;

namespace MovieApi.Application.Features.CQRSDesignPattern.Handlers.SeasonHandlers
{
    public class CreateSeasonCommandHandler
    {
        private readonly MovieContext _context;

        public CreateSeasonCommandHandler(MovieContext context)
        {
            _context = context;
        }

        public async Task Handle(CreateSeasonCommand command)
        {
            _context.Seasons.Add(new Season
            {
                SeriesID = command.SeriesID,
                SeasonNumber = command.SeasonNumber,
                Overview = command.Overview,
                AirDate = command.AirDate,
                PosterImageUrl = command.PosterImageUrl
            });
            await _context.SaveChangesAsync();
        }
    }
}
