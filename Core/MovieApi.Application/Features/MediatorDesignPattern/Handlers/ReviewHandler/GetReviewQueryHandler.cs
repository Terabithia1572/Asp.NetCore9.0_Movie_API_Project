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
            var query = _context.Reviews.AsNoTracking().AsQueryable();
            if (request.MovieId.HasValue) query = query.Where(x => x.MovieID == request.MovieId);
            if (request.SeriesId.HasValue) query = query.Where(x => x.SeriesID == request.SeriesId);
            if (!string.IsNullOrEmpty(request.UserId)) query = query.Where(x => x.UserID == request.UserId);
            if (!string.IsNullOrWhiteSpace(request.Search)) query = query.Where(x => x.ReviewComment != null && x.ReviewComment.Contains(request.Search));
            var values = await query.OrderByDescending(x => x.ReviewDate).ThenByDescending(x => x.ReviewID)
                         .Skip((Math.Max(1, request.Page) - 1) * Math.Clamp(request.PageSize, 1, 100)).Take(Math.Clamp(request.PageSize, 1, 100))
                         .Select(x => new GetReviewQueryResult
                         {
                             IsSpoiler = x.IsSpoiler ?? false,
                             LikeCount = x.LikeCount ?? 0,
                             MovieID = x.MovieID,
                             SeriesID = x.SeriesID,
                             ReviewComment = x.ReviewComment ?? string.Empty,
                             ReviewDate = x.ReviewDate ?? DateTime.UtcNow,
                             ReviewID = x.ReviewID,
                             SentimentScore = x.SentimentScore ?? 0,
                             ReviewStatus = x.ReviewStatus ?? true,
                             UserID = x.UserID,
                             UserRating = x.UserRating ?? 0
                         }).ToListAsync();
            return values;
        }
    }
}
