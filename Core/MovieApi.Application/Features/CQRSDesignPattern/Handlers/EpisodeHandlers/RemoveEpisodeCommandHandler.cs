using MovieApi.Application.Features.CQRSDesignPattern.Commands.EpisodeCommands;
using MovieApi.Persistence.Context;
using System.Threading.Tasks;

namespace MovieApi.Application.Features.CQRSDesignPattern.Handlers.EpisodeHandlers
{
    public class RemoveEpisodeCommandHandler
    {
        private readonly MovieContext _context;

        public RemoveEpisodeCommandHandler(MovieContext context)
        {
            _context = context;
        }

        public async Task Handle(RemoveEpisodeCommand command)
        {
            var value = await _context.Episodes.FindAsync(command.EpisodeID);
            if (value != null)
            {
                _context.Episodes.Remove(value);
                await _context.SaveChangesAsync();
            }
        }
    }
}
