using MovieApi.Application.Features.CQRSDesignPattern.Commands.EpisodeCommands;
using MovieApi.Domain.Entities;
using MovieApi.Persistence.Context;
using System.Threading.Tasks;

namespace MovieApi.Application.Features.CQRSDesignPattern.Handlers.EpisodeHandlers
{
    public class CreateEpisodeCommandHandler
    {
        private readonly MovieContext _context;

        public CreateEpisodeCommandHandler(MovieContext context)
        {
            _context = context;
        }

        public async Task Handle(CreateEpisodeCommand command)
        {
            _context.Episodes.Add(new Episode
            {
                SeasonID = command.SeasonID,
                EpisodeNumber = command.EpisodeNumber,
                EpisodeTitle = command.EpisodeTitle,
                Overview = command.Overview,
                DurationMinutes = command.DurationMinutes,
                AirDate = command.AirDate,
                StillImageUrl = command.StillImageUrl
            });
            await _context.SaveChangesAsync();
        }
    }
}
