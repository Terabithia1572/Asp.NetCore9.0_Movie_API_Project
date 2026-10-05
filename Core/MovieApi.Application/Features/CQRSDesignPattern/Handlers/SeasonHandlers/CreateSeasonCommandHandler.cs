using Microsoft.EntityFrameworkCore;
using MovieApi.Application.Features.CQRSDesignPattern.Commands.SeasonCommands;
using MovieApi.Domain.Entities;
using MovieApi.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieApi.Application.Features.CQRSDesignPattern.Handlers.SeasonHandlers
{
    public class CreateSeasonCommandHandler
    {
        private readonly MovieContext _context;

        public CreateSeasonCommandHandler(MovieContext context)
        {
            _context = context;
        }

        public async Task Handle(CreateSeasonCommand command)
        {
            var seriesExists = await _context.Series.AnyAsync(s => s.SeriesID == command.SeriesID);
            if (!seriesExists)
            {
                throw new KeyNotFoundException($"Series with ID {command.SeriesID} was not found.");
            }

            var seasonExists = await _context.Seasons.AnyAsync(s => s.SeriesID == command.SeriesID && s.SeasonNumber == command.SeasonNumber);
            if (seasonExists)
            {
                throw new InvalidOperationException($"Season number {command.SeasonNumber} already exists for Series ID {command.SeriesID}.");
            }

            _context.Seasons.Add(new Season
            {
                SeriesID = command.SeriesID,
                SeasonNumber = command.SeasonNumber,
                Overview = command.Overview,
                AirDate = command.AirDate,
                PosterImageUrl = command.PosterImageUrl
            });
            await _context.SaveChangesAsync();
        }
    }
}
