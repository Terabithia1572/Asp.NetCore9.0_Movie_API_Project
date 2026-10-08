using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieApi.Application.Features.MediatorDesignPattern.Queries.ReviewQueries;
using MovieApi.Persistence.Context;

namespace Movie.Api.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly MovieContext _context;
        public ReviewsController(IMediator mediator, MovieContext context)
        {
            _mediator = mediator;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> ReviewList([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] int? movieId = null, [FromQuery] int? seriesId = null, [FromQuery] string? userId = null, [FromQuery] string? query = null)
        {
            var records = _context.Reviews.AsNoTracking().AsQueryable();
            if (movieId.HasValue) records = records.Where(x => x.MovieID == movieId);
            if (seriesId.HasValue) records = records.Where(x => x.SeriesID == seriesId);
            if (!string.IsNullOrEmpty(userId)) records = records.Where(x => x.UserID == userId);
            if (!string.IsNullOrWhiteSpace(query)) records = records.Where(x => x.ReviewComment != null && x.ReviewComment.Contains(query));
            var totalCount = await records.CountAsync();
            Response.Headers["X-Total-Count"] = totalCount.ToString();

            var values = await _mediator.Send(new GetReviewQuery { Page = page, PageSize = pageSize, MovieId = movieId, SeriesId = seriesId, UserId = userId, Search = query });
            return Ok(values);
        }

    }
}
