using Microsoft.EntityFrameworkCore;
using MovieApi.Application.Features.CQRSDesignPattern.Commands.EpisodeCommands;
using MovieApi.Domain.Entities;
using MovieApi.Persistence.Context;
using System;
using System.Collections.Generic;
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
            var seasonExists = await _context.Seasons.AnyAsync(s => s.SeasonID == command.SeasonID);
            if (!seasonExists)
            {
                throw new KeyNotFoundException($"Season with ID {command.SeasonID} was not found.");
            }

            var episodeExists = await _context.Episodes.AnyAsync(e => e.SeasonID == command.SeasonID && e.EpisodeNumber == command.EpisodeNumber);
            if (episodeExists)
            {
                throw new InvalidOperationException($"Episode number {command.EpisodeNumber} already exists for Season ID {command.SeasonID}.");
            }

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
