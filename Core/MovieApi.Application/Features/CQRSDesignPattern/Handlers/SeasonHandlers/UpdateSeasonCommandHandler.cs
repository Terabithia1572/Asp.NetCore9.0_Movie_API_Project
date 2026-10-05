using MovieApi.Application.Features.CQRSDesignPattern.Commands.SeasonCommands;
using MovieApi.Persistence.Context;
using System.Threading.Tasks;

namespace MovieApi.Application.Features.CQRSDesignPattern.Handlers.SeasonHandlers
{
    public class UpdateSeasonCommandHandler
    {
        private readonly MovieContext _context;

        public UpdateSeasonCommandHandler(MovieContext context)
        {
            _context = context;
        }

        public async Task Handle(UpdateSeasonCommand command)
        {
            var value = await _context.Seasons.FindAsync(command.SeasonID);
            if (value != null)
            {
                value.SeriesID = command.SeriesID;
                value.SeasonNumber = command.SeasonNumber;
                value.Overview = command.Overview;
                value.AirDate = command.AirDate;
                value.PosterImageUrl = command.PosterImageUrl;
                await _context.SaveChangesAsync();
            }
        }
    }
}
