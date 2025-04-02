using MediatR;
using MovieApi.Application.Features.MediatorDesignPattern.Commands.CastCommands;
using MovieApi.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApi.Application.Features.MediatorDesignPattern.Handlers.CastHandler
{
    public class UpdateCastCommandHandler : IRequestHandler<UpdateCastCommand>
    {
        private readonly MovieContext _movieContext;

        public UpdateCastCommandHandler(MovieContext movieContext)
        {
            _movieContext = movieContext;
        }

        public async Task Handle(UpdateCastCommand request, CancellationToken cancellationToken)
        {
            var values =await _movieContext.Casts.FindAsync(request.CastID);
            values.CastBiography = request.CastBiography;
            values.CastImageURL = request.CastImageURL;
            values.CastName = request.CastName;
            values.CastOverview = request.CastOverview;
            values.CastSurname = request.CastSurname;
            values.CastTitle = request.CastTitle;
            await _movieContext.SaveChangesAsync();

        }
    }
}
