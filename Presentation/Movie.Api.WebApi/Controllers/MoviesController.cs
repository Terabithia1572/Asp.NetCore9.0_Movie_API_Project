using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieApi.Application.Features.CQRSDesignPattern.Commands.MovieCommands;
using MovieApi.Application.Features.CQRSDesignPattern.Handlers.MovieHandlers;
using MovieApi.Application.Features.CQRSDesignPattern.Queries.MovieQueries;
using MovieApi.Persistence.Context;
using System.Linq;
using System.Threading.Tasks;

namespace Movie.Api.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly GetMovieByIDQueryHandler _getMovieByIDQueryHandler;
        private readonly GetMovieQueryHandler _getMovieQueryHandler;
        private readonly CreateMovieCommandHandler _createMovieCommandHandler;
        private readonly UpdateMovieCommandHandler _updateMovieCommandHandler;
        private readonly RemoveMovieCommandHandler _removeMovieCommandHandler;
        private readonly GetMovieWithCategoryQueryHandler _getMovieWithCategoryQueryHandler;
        private readonly MovieContext _context;

        public MoviesController(
            GetMovieByIDQueryHandler getMovieByIDQueryHandler,
            GetMovieQueryHandler getMovieQueryHandler,
            CreateMovieCommandHandler createMovieCommandHandler,
            UpdateMovieCommandHandler updateMovieCommandHandler,
            RemoveMovieCommandHandler removeMovieCommandHandler,
            GetMovieWithCategoryQueryHandler getMovieWithCategoryQueryHandler,
            MovieContext context)
        {
            _getMovieByIDQueryHandler = getMovieByIDQueryHandler;
            _getMovieQueryHandler = getMovieQueryHandler;
            _createMovieCommandHandler = createMovieCommandHandler;
            _updateMovieCommandHandler = updateMovieCommandHandler;
            _removeMovieCommandHandler = removeMovieCommandHandler;
            _getMovieWithCategoryQueryHandler = getMovieWithCategoryQueryHandler;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> MovieList()
        {
            var values = await _getMovieQueryHandler.Handle();
            return Ok(values);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMovie(CreateMovieCommand createMovieCommand)
        {
            await _createMovieCommandHandler.Handle(createMovieCommand);
            return Ok("Film Başarıyla Eklendi..");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            await _removeMovieCommandHandler.Handle(new RemoveMovieCommand(id));
            return Ok("Film Başarıyla Silindi..");
        }

        [HttpGet("GetMovile")]
        public async Task<IActionResult> GetMovie(int id)
        {
            var values = await _getMovieByIDQueryHandler.Handle(new GetMovieByIDQuery(id));
            return Ok(values);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateMovie(UpdateMovieCommand updateMovieCommand)
        {
            await _updateMovieCommandHandler.Handle(updateMovieCommand);
            return Ok("Film Başarıyla Güncellendi..");
        }

        [HttpGet("GetMovieWithCategory")]
        public async Task<IActionResult> GetMovieWithCategory()
        {
            var values = await _getMovieWithCategoryQueryHandler.Handle();
            return Ok(values);
        }

        [HttpGet("{id}/casts")]
        public async Task<IActionResult> GetMovieCasts(int id)
        {
            var castIds = await _context.MovieCasts
                .Where(mc => mc.MovieID == id)
                .Select(mc => mc.CastID)
                .ToListAsync();
            return Ok(castIds);
        }

        [HttpGet("{id}/tags")]
        public async Task<IActionResult> GetMovieTags(int id)
        {
            var tagIds = await _context.MovieTags
                .Where(mt => mt.MovieID == id)
                .Select(mt => mt.TagID)
                .ToListAsync();
            return Ok(tagIds);
        }
    }
}
