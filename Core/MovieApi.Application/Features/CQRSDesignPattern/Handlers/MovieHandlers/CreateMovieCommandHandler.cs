using Microsoft.EntityFrameworkCore;
using MovieApi.Application.Features.CQRSDesignPattern.Commands.MovieCommands;
using MovieApi.Domain.Entities;
using MovieApi.Persistence.Context;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApi.Application.Features.CQRSDesignPattern.Handlers.MovieHandlers
{
    public class CreateMovieCommandHandler
    {
        private readonly MovieContext _context;

        public CreateMovieCommandHandler(MovieContext context)
        {
            _context = context;
        }

        public async Task Handle(CreateMovieCommand createMovieCommand)
        {
            var movie = new Movie
            {
                MovieTitle = createMovieCommand.MovieTitle,
                MovieCoverImageURL = createMovieCommand.MovieCoverImageURL,
                MovieRating = createMovieCommand.MovieRating,
                MovieDescription = createMovieCommand.MovieDescription,
                MovieDuration = createMovieCommand.MovieDuration,
                MovieReleaseDate = createMovieCommand.MovieReleaseDate,
                MovileCreatedYear = createMovieCommand.MovileCreatedYear,
                MovieStatus = createMovieCommand.MovieStatus,
                CategoryID = createMovieCommand.CategoryID
            };

            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();

            if (createMovieCommand.SelectedCastIds != null && createMovieCommand.SelectedCastIds.Any())
            {
                foreach (var castId in createMovieCommand.SelectedCastIds)
                {
                    _context.MovieCasts.Add(new MovieCast { MovieID = movie.MovieID, CastID = castId });
                }
            }

            if (createMovieCommand.SelectedTagIds != null && createMovieCommand.SelectedTagIds.Any())
            {
                foreach (var tagId in createMovieCommand.SelectedTagIds)
                {
                    _context.MovieTags.Add(new MovieTag { MovieID = movie.MovieID, TagID = tagId });
                }
            }

            if ((createMovieCommand.SelectedCastIds != null && createMovieCommand.SelectedCastIds.Any()) ||
                (createMovieCommand.SelectedTagIds != null && createMovieCommand.SelectedTagIds.Any()))
            {
                await _context.SaveChangesAsync();
            }
        }
    }
}
