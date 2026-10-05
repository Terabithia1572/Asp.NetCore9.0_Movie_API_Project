using MovieApi.Application.Features.CQRSDesignPattern.Commands.EpisodeCommands;
using MovieApi.Persistence.Context;
using System.Threading.Tasks;

namespace MovieApi.Application.Features.CQRSDesignPattern.Handlers.EpisodeHandlers
{
    public class UpdateEpisodeCommandHandler
    {
        private readonly MovieContext _context;

        public UpdateEpisodeCommandHandler(MovieContext context)
        {
            _context = context;
        }

        public async Task Handle(UpdateEpisodeCommand command)
        {
            var value = await _context.Episodes.FindAsync(command.EpisodeID);
            if (value != null)
            {
                value.SeasonID = command.SeasonID;
                value.EpisodeNumber = command.EpisodeNumber;
                value.EpisodeTitle = command.EpisodeTitle;
                value.Overview = command.Overview;
                value.DurationMinutes = command.DurationMinutes;
                value.AirDate = command.AirDate;
                value.StillImageUrl = command.StillImageUrl;
                await _context.SaveChangesAsync();
            }
        }
    }
}
