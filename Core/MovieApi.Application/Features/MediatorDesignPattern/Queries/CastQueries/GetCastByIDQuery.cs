using MediatR;
using MovieApi.Application.Features.MediatorDesignPattern.Results.CastResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApi.Application.Features.MediatorDesignPattern.Queries.CastQueries
{
    public class GetCastByIDQuery:IRequest<GetCastByIDQueryResult>
    {
        public int CastID { get; set; } //Oyuncu ID

        public GetCastByIDQuery(int castID)
        {
            CastID = castID;
        }
    }
}
