using MovieApi.Application.Features.QCRSDesignPattern.Commands.MovieCommands;
using MovieApi.Domain.Entities;
using MovieApi.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApi.Application.Features.QCRSDesignPattern.Handlers.MovieHandlers
{
    public class CreateMovieCommandHandler
    {
        private readonly MovieContext _context;

        public CreateMovieCommandHandler(MovieContext context)
        {
            _context = context;
        }
        public async void Handle(CreateMovieCommand createMovieCommand)
        {
            _context.Movies.Add(new Movie
            {
                MovieTitle = createMovieCommand.MovieTitle, //Film adı 
                MovieCoverImageURL = createMovieCommand.MovieCoverImageURL,//Film kapak resmi
                MovieRating = createMovieCommand.MovieRating,//Film puanı
                MovieDescription = createMovieCommand.MovieDescription,//Film açıklaması
                MovieDuration = createMovieCommand.MovieDuration,//Film süresi
                MovieReleaseDate = createMovieCommand.MovieReleaseDate,//Film yayın tarihi
                MovileCreatedYear = createMovieCommand.MovileCreatedYear,//Film çıkış yılı
                MovieStatus = createMovieCommand.MovieStatus //Film durumu
            });
            await _context.SaveChangesAsync();
        }
    }
}
