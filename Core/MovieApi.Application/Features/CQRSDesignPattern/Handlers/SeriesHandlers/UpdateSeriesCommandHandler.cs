using MovieApi.Application.Features.CQRSDesignPattern.Commands.SeriesCommands;
using MovieApi.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApi.Application.Features.CQRSDesignPattern.Handlers.SeriesHandlers
{
    public class UpdateSeriesCommandHandler
    {
        private readonly MovieContext _context;

        public UpdateSeriesCommandHandler(MovieContext context)
        {
            _context = context;
        }

        public async Task Handle(UpdateSeriesCommand command)
        {
            var series = await _context.Series.FindAsync(command.SeriesID);
            if (series != null)
            {
                series.SeriesTitle = command.SeriesTitle;
                series.SeriesCoverImageURL = command.SeriesCoverImageURL;
                series.SeriesRating = command.SeriesRating;
                series.SeriesDescription = command.SeriesDescription;
                series.FirstAirDate = command.FirstAirDate;
                series.SeriesCreatedYear = command.SeriesCreatedYear;
                series.SeriesAverageEpisodeDuration = command.SeriesAverageEpisodeDuration;
                series.SeriesSeasonCount = command.SeriesSeasonCount;
                series.SeriesEpisodeCount = command.SeriesEpisodeCount;
                series.SeriesStatus = command.SeriesStatus;
                series.CategoryID = command.CategoryID;
                await _context.SaveChangesAsync();
            }
        }
    }
}
