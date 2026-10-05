using Microsoft.AspNetCore.Mvc;
using MovieApi.Application.Features.CQRSDesignPattern.Commands.EpisodeCommands;
using MovieApi.Application.Features.CQRSDesignPattern.Handlers.EpisodeHandlers;
using MovieApi.Application.Features.CQRSDesignPattern.Queries.EpisodeQueries;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Movie.Api.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EpisodesController : ControllerBase
    {
        private readonly GetEpisodeQueryHandler _getEpisodeQueryHandler;
        private readonly GetEpisodeByIDQueryHandler _getEpisodeByIDQueryHandler;
        private readonly GetEpisodeBySeasonIDQueryHandler _getEpisodeBySeasonIDQueryHandler;
        private readonly CreateEpisodeCommandHandler _createEpisodeCommandHandler;
        private readonly UpdateEpisodeCommandHandler _updateEpisodeCommandHandler;
        private readonly RemoveEpisodeCommandHandler _removeEpisodeCommandHandler;

        public EpisodesController(
            GetEpisodeQueryHandler getEpisodeQueryHandler,
            GetEpisodeByIDQueryHandler getEpisodeByIDQueryHandler,
            GetEpisodeBySeasonIDQueryHandler getEpisodeBySeasonIDQueryHandler,
            CreateEpisodeCommandHandler createEpisodeCommandHandler,
            UpdateEpisodeCommandHandler updateEpisodeCommandHandler,
            RemoveEpisodeCommandHandler removeEpisodeCommandHandler)
        {
            _getEpisodeQueryHandler = getEpisodeQueryHandler;
            _getEpisodeByIDQueryHandler = getEpisodeByIDQueryHandler;
            _getEpisodeBySeasonIDQueryHandler = getEpisodeBySeasonIDQueryHandler;
            _createEpisodeCommandHandler = createEpisodeCommandHandler;
            _updateEpisodeCommandHandler = updateEpisodeCommandHandler;
            _removeEpisodeCommandHandler = removeEpisodeCommandHandler;
        }

        [HttpGet]
        public async Task<IActionResult> EpisodeList()
        {
            var values = await _getEpisodeQueryHandler.Handle();
            return Ok(values);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEpisode(int id)
        {
            var value = await _getEpisodeByIDQueryHandler.Handle(new GetEpisodeByIDQuery(id));
            if (value == null)
            {
                return NotFound($"Episode with ID {id} was not found.");
            }
            return Ok(value);
        }

        [HttpGet("season/{seasonId}")]
        public async Task<IActionResult> GetEpisodesBySeasonID(int seasonId)
        {
            try
            {
                var values = await _getEpisodeBySeasonIDQueryHandler.Handle(new GetEpisodeBySeasonIDQuery(seasonId));
                return Ok(values);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateEpisode([FromBody] CreateEpisodeCommand createEpisodeCommand)
        {
            try
            {
                await _createEpisodeCommandHandler.Handle(createEpisodeCommand);
                return Ok("Episode successfully created.");
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
        public async Task<IActionResult> UpdateEpisode([FromBody] UpdateEpisodeCommand updateEpisodeCommand)
        {
            try
            {
                await _updateEpisodeCommandHandler.Handle(updateEpisodeCommand);
                return Ok("Episode successfully updated.");
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
        public async Task<IActionResult> DeleteEpisode(int id)
        {
            try
            {
                await _removeEpisodeCommandHandler.Handle(new RemoveEpisodeCommand(id));
                return Ok("Episode successfully deleted.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
