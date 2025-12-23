using MovieApi.Application.Features.CQRSDesignPattern.Commands.SeriesCommands;
using MovieApi.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            _context.Series.Add(new()
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
            });
            await _context.SaveChangesAsync();
        }
    }
}
