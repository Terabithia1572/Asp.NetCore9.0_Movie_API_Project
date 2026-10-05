using Microsoft.EntityFrameworkCore;
using MovieApi.Application.Features.CQRSDesignPattern.Commands.SeasonCommands;
using MovieApi.Persistence.Context;
using System;
using System.Collections.Generic;
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
            if (value == null)
            {
                throw new KeyNotFoundException($"Season with ID {command.SeasonID} was not found.");
            }

            var seriesExists = await _context.Series.AnyAsync(s => s.SeriesID == command.SeriesID);
            if (!seriesExists)
            {
                throw new KeyNotFoundException($"Series with ID {command.SeriesID} was not found.");
            }

            var duplicateExists = await _context.Seasons.AnyAsync(s => s.SeriesID == command.SeriesID && s.SeasonNumber == command.SeasonNumber && s.SeasonID != command.SeasonID);
            if (duplicateExists)
            {
                throw new InvalidOperationException($"Season number {command.SeasonNumber} already exists for Series ID {command.SeriesID}.");
            }

            value.SeriesID = command.SeriesID;
            value.SeasonNumber = command.SeasonNumber;
            value.Overview = command.Overview;
            value.AirDate = command.AirDate;
            value.PosterImageUrl = command.PosterImageUrl;
            await _context.SaveChangesAsync();
        }
    }
}
