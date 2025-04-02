using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieApi.Application.Features.QCRSDesignPattern.Commands.MovieCommands;
using MovieApi.Application.Features.QCRSDesignPattern.Handlers.MovieHandlers;
using MovieApi.Application.Features.QCRSDesignPattern.Queries.MovieQueries;

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

        public MoviesController(GetMovieByIDQueryHandler getMovieByIDQueryHandler, GetMovieQueryHandler getMovieQueryHandler, CreateMovieCommandHandler createMovieCommandHandler, UpdateMovieCommandHandler updateMovieCommandHandler, RemoveMovieCommandHandler removeMovieCommandHandler)
        {
            _getMovieByIDQueryHandler = getMovieByIDQueryHandler;
            _getMovieQueryHandler = getMovieQueryHandler;
            _createMovieCommandHandler = createMovieCommandHandler;
            _updateMovieCommandHandler = updateMovieCommandHandler;
            _removeMovieCommandHandler = removeMovieCommandHandler;
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
        [HttpDelete]
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
    }
}
