using MediatR;
using Microsoft.EntityFrameworkCore;
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
    public class GetCastQueryHandler : IRequestHandler<GetCastQuery, List<GetCastQueryResult>>
    {
        private readonly MovieContext _movieContext;

        public GetCastQueryHandler(MovieContext movieContext)
        {
            _movieContext = movieContext;
        }

        public async Task<List<GetCastQueryResult>> Handle(GetCastQuery request, CancellationToken cancellationToken)
        {
            var values = await _movieContext.Casts.ToListAsync();
            return values.Select(x=>new GetCastQueryResult
            {
                CastID = x.CastID,
                CastBiography = x.CastBiography,
                CastImageURL = x.CastImageURL,
                CastName = x.CastName,
                CastOverview = x.CastOverview,
                CastSurname = x.CastSurname,
                CastTitle = x.CastTitle
                
            }).ToList();
        } //AsNoTracking: üzerinde sadece read yapılacak işlemlerde kullandığımız bir ef core fonksiyonudur.
          //Normalde ef core entity üzerinde tracking başlatır ve yapılan değişikleri takip ederek savechanges ile veritabanına yansıtır.
          //Ama read işleminde savechanges gerek olmadığı ve entity üzerinde bir güncelleme yapmadığımız için performans artışı için AsNoTracking kullanırız.
    }
}
