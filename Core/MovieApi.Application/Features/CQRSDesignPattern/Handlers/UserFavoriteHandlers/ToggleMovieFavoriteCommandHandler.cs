using Microsoft.EntityFrameworkCore;
using MovieApi.Application.Features.CQRSDesignPattern.Commands.UserFavoriteCommands;
using MovieApi.Domain.Entities;
using MovieApi.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieApi.Application.Features.CQRSDesignPattern.Handlers.UserFavoriteHandlers
{
    public class ToggleMovieFavoriteCommandHandler
    {
        private readonly MovieContext _context;

        public ToggleMovieFavoriteCommandHandler(MovieContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(ToggleMovieFavoriteCommand command)
        {
            var userExists = await _context.Users.AnyAsync(u => u.Id == command.UserId);
            if (!userExists)
            {
                throw new KeyNotFoundException($"User with ID '{command.UserId}' was not found.");
            }

            var movieExists = await _context.Movies.AnyAsync(m => m.MovieID == command.MovieId);
            if (!movieExists)
            {
                throw new KeyNotFoundException($"Movie with ID {command.MovieId} was not found.");
            }

            var existing = await _context.UserFavorites
                .FirstOrDefaultAsync(x => x.UserId == command.UserId && x.MovieID == command.MovieId);

            if (existing != null)
            {
                _context.UserFavorites.Remove(existing);
                await _context.SaveChangesAsync();
                return false; // Removed from favorites
            }

            _context.UserFavorites.Add(new UserFavorite
            {
                UserId = command.UserId,
                MovieID = command.MovieId,
                CreatedDate = DateTime.UtcNow
            });
            await _context.SaveChangesAsync();
            return true; // Added to favorites
        }
    }
}
