using MediatR;
using MovieApi.Application.Features.MediatorDesignPattern.Commands.TagCommands;
using MovieApi.Domain.Entities;
using MovieApi.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApi.Application.Features.MediatorDesignPattern.Handlers.TagHandler
{
    public class CreateTagCommandHandler : IRequestHandler<CreateTagCommand>
    {
        private readonly MovieContext _movieContext;

        public CreateTagCommandHandler(MovieContext movieContext)
        {
            _movieContext = movieContext;
        }

        public async Task Handle(CreateTagCommand request, CancellationToken cancellationToken)
        {
           await _movieContext.Tags.AddAsync(new Tag
            {
                TagTitle = request.TagTitle
            });
            await _movieContext.SaveChangesAsync();
        }
    }
}
