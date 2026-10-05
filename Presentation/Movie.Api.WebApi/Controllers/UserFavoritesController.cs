using Microsoft.AspNetCore.Mvc;
using MovieApi.Application.Features.CQRSDesignPattern.Commands.UserFavoriteCommands;
using MovieApi.Application.Features.CQRSDesignPattern.Handlers.UserFavoriteHandlers;
using MovieApi.Application.Features.CQRSDesignPattern.Queries.UserFavoriteQueries;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Movie.Api.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserFavoritesController : ControllerBase
    {
        private readonly GetUserFavoriteMoviesQueryHandler _getUserFavoriteMoviesQueryHandler;
        private readonly GetUserFavoriteSeriesQueryHandler _getUserFavoriteSeriesQueryHandler;
        private readonly IsMovieFavoritedQueryHandler _isMovieFavoritedQueryHandler;
        private readonly IsSeriesFavoritedQueryHandler _isSeriesFavoritedQueryHandler;
        private readonly ToggleMovieFavoriteCommandHandler _toggleMovieFavoriteCommandHandler;
        private readonly ToggleSeriesFavoriteCommandHandler _toggleSeriesFavoriteCommandHandler;
        private readonly RemoveUserFavoriteCommandHandler _removeUserFavoriteCommandHandler;

        public UserFavoritesController(
            GetUserFavoriteMoviesQueryHandler getUserFavoriteMoviesQueryHandler,
            GetUserFavoriteSeriesQueryHandler getUserFavoriteSeriesQueryHandler,
            IsMovieFavoritedQueryHandler isMovieFavoritedQueryHandler,
            IsSeriesFavoritedQueryHandler isSeriesFavoritedQueryHandler,
            ToggleMovieFavoriteCommandHandler toggleMovieFavoriteCommandHandler,
            ToggleSeriesFavoriteCommandHandler toggleSeriesFavoriteCommandHandler,
            RemoveUserFavoriteCommandHandler removeUserFavoriteCommandHandler)
        {
            _getUserFavoriteMoviesQueryHandler = getUserFavoriteMoviesQueryHandler;
            _getUserFavoriteSeriesQueryHandler = getUserFavoriteSeriesQueryHandler;
            _isMovieFavoritedQueryHandler = isMovieFavoritedQueryHandler;
            _isSeriesFavoritedQueryHandler = isSeriesFavoritedQueryHandler;
            _toggleMovieFavoriteCommandHandler = toggleMovieFavoriteCommandHandler;
            _toggleSeriesFavoriteCommandHandler = toggleSeriesFavoriteCommandHandler;
            _removeUserFavoriteCommandHandler = removeUserFavoriteCommandHandler;
        }

        [HttpGet("movies/{userId}")]
        public async Task<IActionResult> GetUserFavoriteMovies(string userId)
        {
            var values = await _getUserFavoriteMoviesQueryHandler.Handle(new GetUserFavoriteMoviesQuery(userId));
            return Ok(values);
        }

        [HttpGet("series/{userId}")]
        public async Task<IActionResult> GetUserFavoriteSeries(string userId)
        {
            var values = await _getUserFavoriteSeriesQueryHandler.Handle(new GetUserFavoriteSeriesQuery(userId));
            return Ok(values);
        }

        [HttpGet("is-movie-favorited")]
        public async Task<IActionResult> IsMovieFavorited([FromQuery] string userId, [FromQuery] int movieId)
        {
            var isFavorited = await _isMovieFavoritedQueryHandler.Handle(new IsMovieFavoritedQuery(userId, movieId));
            return Ok(isFavorited);
        }

        [HttpGet("is-series-favorited")]
        public async Task<IActionResult> IsSeriesFavorited([FromQuery] string userId, [FromQuery] int seriesId)
        {
            var isFavorited = await _isSeriesFavoritedQueryHandler.Handle(new IsSeriesFavoritedQuery(userId, seriesId));
            return Ok(isFavorited);
        }

        [HttpPost("toggle-movie")]
        public async Task<IActionResult> ToggleMovieFavorite([FromBody] ToggleMovieFavoriteCommand command)
        {
            try
            {
                var isFavorited = await _toggleMovieFavoriteCommandHandler.Handle(command);
                return Ok(new { isFavorited, message = isFavorited ? "Movie added to favorites." : "Movie removed from favorites." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("toggle-series")]
        public async Task<IActionResult> ToggleSeriesFavorite([FromBody] ToggleSeriesFavoriteCommand command)
        {
            try
            {
                var isFavorited = await _toggleSeriesFavoriteCommandHandler.Handle(command);
                return Ok(new { isFavorited, message = isFavorited ? "Series added to favorites." : "Series removed from favorites." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserFavorite(int id)
        {
            try
            {
                await _removeUserFavoriteCommandHandler.Handle(new RemoveUserFavoriteCommand(id));
                return Ok("Favorite deleted successfully.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
