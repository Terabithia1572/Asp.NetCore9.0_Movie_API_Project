using Microsoft.EntityFrameworkCore;
using MovieApi.Application.Features.CQRSDesignPattern.Commands.MovieCommands;
using MovieApi.Domain.Entities;
using MovieApi.Persistence.Context;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApi.Application.Features.CQRSDesignPattern.Handlers.MovieHandlers
{
    public class UpdateMovieCommandHandler
    {
        private readonly MovieContext _context;

        public UpdateMovieCommandHandler(MovieContext context)
        {
            _context = context;
        }

        public async Task Handle(UpdateMovieCommand updateMovieCommand)
        {
            var values = await _context.Movies.FindAsync(updateMovieCommand.MovieID);
            if (values == null) return;

            values.MovieStatus = updateMovieCommand.MovieStatus;
            values.MovieRating = updateMovieCommand.MovieRating;
            values.MovieDuration = updateMovieCommand.MovieDuration;
            values.MovieDescription = updateMovieCommand.MovieDescription;
            values.MovieTitle = updateMovieCommand.MovieTitle;
            values.MovieReleaseDate = updateMovieCommand.MovieReleaseDate;
            values.MovieCoverImageURL = updateMovieCommand.MovieCoverImageURL;
            values.MovileCreatedYear = updateMovieCommand.MovileCreatedYear;
            if (updateMovieCommand.CategoryID > 0)
            {
                values.CategoryID = updateMovieCommand.CategoryID;
            }

            // Sync Casts
            var existingCasts = _context.MovieCasts.Where(mc => mc.MovieID == updateMovieCommand.MovieID);
            _context.MovieCasts.RemoveRange(existingCasts);

            if (updateMovieCommand.SelectedCastIds != null && updateMovieCommand.SelectedCastIds.Any())
            {
                foreach (var castId in updateMovieCommand.SelectedCastIds)
                {
                    _context.MovieCasts.Add(new MovieCast { MovieID = updateMovieCommand.MovieID, CastID = castId });
                }
            }

            // Sync Tags
            var existingTags = _context.MovieTags.Where(mt => mt.MovieID == updateMovieCommand.MovieID);
            _context.MovieTags.RemoveRange(existingTags);

            if (updateMovieCommand.SelectedTagIds != null && updateMovieCommand.SelectedTagIds.Any())
            {
                foreach (var tagId in updateMovieCommand.SelectedTagIds)
                {
                    _context.MovieTags.Add(new MovieTag { MovieID = updateMovieCommand.MovieID, TagID = tagId });
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
