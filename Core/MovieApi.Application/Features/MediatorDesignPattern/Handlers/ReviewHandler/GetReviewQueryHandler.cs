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
            var values = await _context.Reviews.ToListAsync();
            return values.Select(x => new GetReviewQueryResult
            {
                ReviewID = x.ReviewID,
                ReviewComment = x.ReviewComment,
                UserRating = x.UserRating,
                ReviewDate = x.ReviewDate,
                ReviewStatus = x.ReviewStatus,
                UserID = x.UserID,
                MovieID = x.MovieID,
                Movie = x.Movie,
                IsSpoiler = x.IsSpoiler,
                LikeCount = x.LikeCount,
                SentimentScore = x.SentimentScore
            }).ToList();
        }
    }
}
