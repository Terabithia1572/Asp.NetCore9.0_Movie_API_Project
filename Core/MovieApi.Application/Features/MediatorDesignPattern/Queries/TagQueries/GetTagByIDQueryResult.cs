using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApi.Application.Features.MediatorDesignPattern.Queries.TagQueries
{
    public class GetTagByIDQueryResult:IRequest<GetTagByIDQueryResult>
    {
        public int TagID { get; set; } //Etiket ID

        public GetTagByIDQueryResult(int tagID)
        {
            TagID = tagID;
        }
    }
}
