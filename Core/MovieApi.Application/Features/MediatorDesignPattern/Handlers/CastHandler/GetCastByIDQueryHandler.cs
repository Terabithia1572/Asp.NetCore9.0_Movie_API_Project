using MediatR;
using MovieApi.Application.Features.MediatorDesignPattern.Queries.CastQueries;
using MovieApi.Application.Features.MediatorDesignPattern.Results.CastResults;
using MovieApi.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApi.Application.Features.MediatorDesignPattern.Handlers.CastHandler
{
    public class GetCastByIDQueryHandler : IRequestHandler<GetCastByIDQuery, GetCastByIDQueryResult>
    {
        private readonly MovieContext _movieContext;

        public GetCastByIDQueryHandler(MovieContext movieContext)
        {
            _movieContext = movieContext;
        }

        public async Task<GetCastByIDQueryResult> Handle(GetCastByIDQuery request, CancellationToken cancellationToken)
        {
            var values =await _movieContext.Casts.FindAsync(request.CastID);
            return new GetCastByIDQueryResult
            {
                CastID = values.CastID,
                CastBiography = values.CastBiography,
                CastImageURL = values.CastImageURL,
                CastName = values.CastName,
                CastOverview = values.CastOverview,
                CastSurname = values.CastSurname,
                CastTitle = values.CastTitle

            };
        }
    }
}
