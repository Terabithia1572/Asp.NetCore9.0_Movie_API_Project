using MovieApi.Application.Features.CQRSDesignPattern.Commands.SeasonCommands;
using MovieApi.Persistence.Context;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieApi.Application.Features.CQRSDesignPattern.Handlers.SeasonHandlers
{
    public class RemoveSeasonCommandHandler
    {
        private readonly MovieContext _context;

        public RemoveSeasonCommandHandler(MovieContext context)
        {
            _context = context;
        }

        public async Task Handle(RemoveSeasonCommand command)
        {
            var value = await _context.Seasons.FindAsync(command.SeasonID);
            if (value == null)
            {
                throw new KeyNotFoundException($"Season with ID {command.SeasonID} was not found.");
            }

            _context.Seasons.Remove(value);
            await _context.SaveChangesAsync();
        }
    }
}
