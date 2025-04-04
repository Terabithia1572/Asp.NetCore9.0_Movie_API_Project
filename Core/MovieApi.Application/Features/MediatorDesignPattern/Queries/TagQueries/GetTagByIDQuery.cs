using MediatR;
using MovieApi.Application.Features.MediatorDesignPattern.Results.TagResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApi.Application.Features.MediatorDesignPattern.Queries.TagQueries
{
    public class GetTagByIDQuery : IRequest<GetTagByIDQueryResult>
    {
        public int TagID { get; set; }

        public GetTagByIDQuery(int tagId)
        {
            TagID = tagId;
        }
    }
}
