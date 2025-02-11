using MovieApi.Application.Features.QCRSDesignPattern.Commands.MovieCommands;
using MovieApi.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApi.Application.Features.QCRSDesignPattern.Handlers.MovieHandlers
{
    public class RemoveMovieCommandHandler
    {
        private readonly MovieContext _context;

        public RemoveMovieCommandHandler(MovieContext context)
        {
            _context = context;
        }

        public async void Handle(RemoveMovieCommand removeMovieCommand)
        {
            var values = _context.Movies.Find(removeMovieCommand.MovieID);
            _context.Movies.Remove(values);
            await _context.SaveChangesAsync();
        }
    }
}
