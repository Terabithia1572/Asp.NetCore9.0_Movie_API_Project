using Microsoft.EntityFrameworkCore;
using MovieApi.Application.Features.CQRSDesignPattern.Commands.SeriesCommands;
using MovieApi.Domain.Entities;
using MovieApi.Persistence.Context;
using System.Linq;
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
            if (series == null) return;

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
            if (command.CategoryID > 0)
            {
                series.CategoryID = command.CategoryID;
            }

            // Sync Casts
            var existingCasts = _context.SeriesCasts.Where(sc => sc.SeriesID == command.SeriesID);
            _context.SeriesCasts.RemoveRange(existingCasts);

            if (command.SelectedCastIds != null && command.SelectedCastIds.Any())
            {
                foreach (var castId in command.SelectedCastIds)
                {
                    _context.SeriesCasts.Add(new SeriesCast { SeriesID = command.SeriesID, CastID = castId });
                }
            }

            // Sync Tags
            var existingTags = _context.SeriesTags.Where(st => st.SeriesID == command.SeriesID);
            _context.SeriesTags.RemoveRange(existingTags);

            if (command.SelectedTagIds != null && command.SelectedTagIds.Any())
            {
                foreach (var tagId in command.SelectedTagIds)
                {
                    _context.SeriesTags.Add(new SeriesTag { SeriesID = command.SeriesID, TagID = tagId });
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
