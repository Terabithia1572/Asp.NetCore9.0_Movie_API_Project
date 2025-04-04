using MediatR;
using MovieApi.Application.Features.MediatorDesignPattern.Queries.TagQueries;
using MovieApi.Application.Features.MediatorDesignPattern.Results.CastResults;
using MovieApi.Application.Features.MediatorDesignPattern.Results.TagResults;
using MovieApi.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApi.Application.Features.MediatorDesignPattern.Handlers.TagHandler
{
    public class GetTagByIDQueryHandler : IRequestHandler<GetTagByIDQuery, GetTagByIDQueryResult>
    {
        private readonly MovieContext _movieContext;

        public GetTagByIDQueryHandler(MovieContext movieContext)
        {
            _movieContext = movieContext;
        }

        public async Task<GetTagByIDQueryResult> Handle(GetTagByIDQuery request, CancellationToken cancellationToken)
        {
           var values = await _movieContext.Tags.FindAsync(request.TagID);
           
                return new GetTagByIDQueryResult
                {
                   
                    TagTitle = values.TagTitle
                };
            }
           
        }
    }

