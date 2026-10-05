using Microsoft.EntityFrameworkCore;
using MovieApi.Application.Features.CQRSDesignPattern.Commands.SeriesCommands;
using MovieApi.Domain.Entities;
using MovieApi.Persistence.Context;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApi.Application.Features.CQRSDesignPattern.Handlers.SeriesHandlers
{
    public class CreateSeriesCommandHandler
    {
        private readonly MovieContext _context;

        public CreateSeriesCommandHandler(MovieContext context)
        {
            _context = context;
        }

        public async Task Handle(CreateSeriesCommand createSeriesCommand)
        {
            var series = new MovieApi.Domain.Entities.Series
            {
                SeriesTitle = createSeriesCommand.SeriesTitle,
                SeriesCoverImageURL = createSeriesCommand.SeriesCoverImageURL,
                SeriesRating = createSeriesCommand.SeriesRating,
                SeriesDescription = createSeriesCommand.SeriesDescription,
                FirstAirDate = createSeriesCommand.FirstAirDate,
                SeriesCreatedYear = createSeriesCommand.SeriesCreatedYear,
                SeriesAverageEpisodeDuration = createSeriesCommand.SeriesAverageEpisodeDuration,
                SeriesSeasonCount = createSeriesCommand.SeriesSeasonCount,
                SeriesEpisodeCount = createSeriesCommand.SeriesEpisodeCount,
                SeriesStatus = createSeriesCommand.SeriesStatus,
                CategoryID = createSeriesCommand.CategoryID
            };

            _context.Series.Add(series);
            await _context.SaveChangesAsync();

            if (createSeriesCommand.SelectedCastIds != null && createSeriesCommand.SelectedCastIds.Any())
            {
                foreach (var castId in createSeriesCommand.SelectedCastIds)
                {
                    _context.SeriesCasts.Add(new SeriesCast { SeriesID = series.SeriesID, CastID = castId });
                }
            }

            if (createSeriesCommand.SelectedTagIds != null && createSeriesCommand.SelectedTagIds.Any())
            {
                foreach (var tagId in createSeriesCommand.SelectedTagIds)
                {
                    _context.SeriesTags.Add(new SeriesTag { SeriesID = series.SeriesID, TagID = tagId });
                }
            }

            if ((createSeriesCommand.SelectedCastIds != null && createSeriesCommand.SelectedCastIds.Any()) ||
                (createSeriesCommand.SelectedTagIds != null && createSeriesCommand.SelectedTagIds.Any()))
            {
                await _context.SaveChangesAsync();
            }
        }
    }
}
