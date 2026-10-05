using Microsoft.AspNetCore.Mvc;
using MovieApi.Application.Features.CQRSDesignPattern.Commands.SeasonCommands;
using MovieApi.Application.Features.CQRSDesignPattern.Handlers.SeasonHandlers;
using MovieApi.Application.Features.CQRSDesignPattern.Queries.SeasonQueries;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Movie.Api.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeasonsController : ControllerBase
    {
        private readonly GetSeasonQueryHandler _getSeasonQueryHandler;
        private readonly GetSeasonByIDQueryHandler _getSeasonByIDQueryHandler;
        private readonly GetSeasonBySeriesIDQueryHandler _getSeasonBySeriesIDQueryHandler;
        private readonly CreateSeasonCommandHandler _createSeasonCommandHandler;
        private readonly UpdateSeasonCommandHandler _updateSeasonCommandHandler;
        private readonly RemoveSeasonCommandHandler _removeSeasonCommandHandler;

        public SeasonsController(
            GetSeasonQueryHandler getSeasonQueryHandler,
            GetSeasonByIDQueryHandler getSeasonByIDQueryHandler,
            GetSeasonBySeriesIDQueryHandler getSeasonBySeriesIDQueryHandler,
            CreateSeasonCommandHandler createSeasonCommandHandler,
            UpdateSeasonCommandHandler updateSeasonCommandHandler,
            RemoveSeasonCommandHandler removeSeasonCommandHandler)
        {
            _getSeasonQueryHandler = getSeasonQueryHandler;
            _getSeasonByIDQueryHandler = getSeasonByIDQueryHandler;
            _getSeasonBySeriesIDQueryHandler = getSeasonBySeriesIDQueryHandler;
            _createSeasonCommandHandler = createSeasonCommandHandler;
            _updateSeasonCommandHandler = updateSeasonCommandHandler;
            _removeSeasonCommandHandler = removeSeasonCommandHandler;
        }

        [HttpGet]
        public async Task<IActionResult> SeasonList()
        {
            var values = await _getSeasonQueryHandler.Handle();
            return Ok(values);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSeason(int id)
        {
            var value = await _getSeasonByIDQueryHandler.Handle(new GetSeasonByIDQuery(id));
            if (value == null)
            {
                return NotFound($"Season with ID {id} was not found.");
            }
            return Ok(value);
        }

        [HttpGet("series/{seriesId}")]
        public async Task<IActionResult> GetSeasonsBySeriesID(int seriesId)
        {
            try
            {
                var values = await _getSeasonBySeriesIDQueryHandler.Handle(new GetSeasonBySeriesIDQuery(seriesId));
                return Ok(values);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateSeason([FromBody] CreateSeasonCommand createSeasonCommand)
        {
            try
            {
                await _createSeasonCommandHandler.Handle(createSeasonCommand);
                return Ok("Season successfully created.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateSeason([FromBody] UpdateSeasonCommand updateSeasonCommand)
        {
            try
            {
                await _updateSeasonCommandHandler.Handle(updateSeasonCommand);
                return Ok("Season successfully updated.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSeason(int id)
        {
            try
            {
                await _removeSeasonCommandHandler.Handle(new RemoveSeasonCommand(id));
                return Ok("Season successfully deleted.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
