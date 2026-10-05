using Microsoft.EntityFrameworkCore;
using MovieApi.Application.Features.CQRSDesignPattern.Commands.UserFavoriteCommands;
using MovieApi.Domain.Entities;
using MovieApi.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieApi.Application.Features.CQRSDesignPattern.Handlers.UserFavoriteHandlers
{
    public class ToggleSeriesFavoriteCommandHandler
    {
        private readonly MovieContext _context;

        public ToggleSeriesFavoriteCommandHandler(MovieContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(ToggleSeriesFavoriteCommand command)
        {
            var userExists = await _context.Users.AnyAsync(u => u.Id == command.UserId);
            if (!userExists)
            {
                throw new KeyNotFoundException($"User with ID '{command.UserId}' was not found.");
            }

            var seriesExists = await _context.Series.AnyAsync(s => s.SeriesID == command.SeriesId);
            if (!seriesExists)
            {
                throw new KeyNotFoundException($"Series with ID {command.SeriesId} was not found.");
            }

            var existing = await _context.UserFavorites
                .FirstOrDefaultAsync(x => x.UserId == command.UserId && x.SeriesID == command.SeriesId);

            if (existing != null)
            {
                _context.UserFavorites.Remove(existing);
                await _context.SaveChangesAsync();
                return false; // Removed from favorites
            }

            _context.UserFavorites.Add(new UserFavorite
            {
                UserId = command.UserId,
                SeriesID = command.SeriesId,
                CreatedDate = DateTime.UtcNow
            });
            await _context.SaveChangesAsync();
            return true; // Added to favorites
        }
    }
}
