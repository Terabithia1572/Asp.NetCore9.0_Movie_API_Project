using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieApi.Application.Features.MediatorDesignPattern.Queries.TagQueries;
using MovieApi.Application.Features.MediatorDesignPattern.Results.TagResults;
using MovieApi.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApi.Application.Features.MediatorDesignPattern.Handlers.TagHandler
{
    public class GetTagQueryHandler : IRequestHandler<GetTagQuery, List<GetTagQueryResult>>
    {
        private readonly MovieContext _movieContext;

        public GetTagQueryHandler(MovieContext movieContext)
        {
            _movieContext = movieContext;
        }

        public async Task<List<GetTagQueryResult>> Handle(GetTagQuery request, CancellationToken cancellationToken)
        {
            var values = await _movieContext.Tags.ToListAsync();
            return values.Select(x => new GetTagQueryResult
            {
                TagID = x.TagID,
                TagTitle = x.TagTitle

            }).ToList();
        } //AsNoTracking: üzerinde sadece read yapılacak işlemlerde kullandığımız bir ef core fonksiyonudur.
          //Normalde ef core entity üzerinde tracking başlatır ve yapılan değişikleri takip ederek savechanges ile veritabanına yansıtır.
          //Ama read işleminde savechanges gerek olmadığı ve entity üzerinde bir güncelleme yapmadığımız için performans artışı için AsNoTracking kullanırız.
    }
}
