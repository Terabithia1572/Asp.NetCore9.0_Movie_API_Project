using MediatR;
using MovieApi.Application.Features.MediatorDesignPattern.Results.ReviewResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApi.Application.Features.MediatorDesignPattern.Queries.ReviewQueries
{
    public class GetReviewQuery:IRequest<List<GetReviewQueryResult>>
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int? MovieId { get; set; }
        public int? SeriesId { get; set; }
        public string? UserId { get; set; }
        public string? Search { get; set; }
    }
}
