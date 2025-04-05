using MediatR;
using MovieApi.Application.Features.MediatorDesignPattern.Commands.CastCommands;
using MovieApi.Domain.Entities;
using MovieApi.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApi.Application.Features.MediatorDesignPattern.Handlers.CastHandler
{
    public class CreateCastCommandHandler : IRequestHandler<CreateCastCommand>
    {
        private readonly MovieContext _movieContext;

        public CreateCastCommandHandler(MovieContext movieContext)
        {
            _movieContext = movieContext;
        }

        public async Task Handle(CreateCastCommand request, CancellationToken cancellationToken)
        {
           await _movieContext.Casts.AddAsync(new Cast
            {
                CastBiography = request.CastBiography,
                CastImageURL = request.CastImageURL,
                CastName = request.CastName,
                CastOverview = request.CastOverview,
                CastSurname = request.CastSurname,
                CastTitle = request.CastTitle

            });
            await _movieContext.SaveChangesAsync();
        }
    }
}
