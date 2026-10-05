using Microsoft.EntityFrameworkCore;
using MovieApi.Application.Features.CQRSDesignPattern.Commands.EpisodeCommands;
using MovieApi.Persistence.Context;
using System;
using System.Collections.Generic;
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
            if (value == null)
            {
                throw new KeyNotFoundException($"Episode with ID {command.EpisodeID} was not found.");
            }

            var seasonExists = await _context.Seasons.AnyAsync(s => s.SeasonID == command.SeasonID);
            if (!seasonExists)
            {
                throw new KeyNotFoundException($"Season with ID {command.SeasonID} was not found.");
            }

            var duplicateExists = await _context.Episodes.AnyAsync(e => e.SeasonID == command.SeasonID && e.EpisodeNumber == command.EpisodeNumber && e.EpisodeID != command.EpisodeID);
            if (duplicateExists)
            {
                throw new InvalidOperationException($"Episode number {command.EpisodeNumber} already exists for Season ID {command.SeasonID}.");
            }

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
