using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieApi.Application.Features.MediatorDesignPattern.Queries.ReviewQueries;
using MovieApi.Application.Features.MediatorDesignPattern.Results.ReviewResult;
using MovieApi.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApi.Application.Features.MediatorDesignPattern.Handlers.ReviewHandler
{
    public class GetReviewQueryHandler : IRequestHandler<GetReviewQuery, List<GetReviewQueryResult>>
    {
        private readonly MovieContext _context;

        public GetReviewQueryHandler(MovieContext context)
        {
            _context = context;
        }

        public async Task<List<GetReviewQueryResult>> Handle(GetReviewQuery request, CancellationToken cancellationToken)
        {
            var values = await _context.Reviews.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize)
                         .Select(x => new GetReviewQueryResult
                         {
                             IsSpoiler = x.IsSpoiler,
                             LikeCount = x.LikeCount,
                             MovieID = x.MovieID,
                             ReviewComment = x.ReviewComment,
                             ReviewDate = x.ReviewDate,
                             ReviewID = x.ReviewID,
                             SentimentScore = x.SentimentScore,
                             ReviewStatus = x.ReviewStatus,
                             UserID = x.UserID,
                             UserRating = x.UserRating
                         }).ToListAsync();
            return values;
        }
    }
}
