using MovieApi.Application.Features.CQRSDesignPattern.Commands.UserFavoriteCommands;
using MovieApi.Persistence.Context;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieApi.Application.Features.CQRSDesignPattern.Handlers.UserFavoriteHandlers
{
    public class RemoveUserFavoriteCommandHandler
    {
        private readonly MovieContext _context;

        public RemoveUserFavoriteCommandHandler(MovieContext context)
        {
            _context = context;
        }

        public async Task Handle(RemoveUserFavoriteCommand command)
        {
            var existing = await _context.UserFavorites.FindAsync(command.UserFavoriteID);
            if (existing == null)
            {
                throw new KeyNotFoundException($"Favorite item with ID {command.UserFavoriteID} was not found.");
            }

            _context.UserFavorites.Remove(existing);
            await _context.SaveChangesAsync();
        }
    }
}
