using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieApi.Application.Features.CQRSDesignPattern.Commands.SeriesCommands;
using MovieApi.Application.Features.CQRSDesignPattern.Handlers.SeriesHandlers;
using MovieApi.Application.Features.CQRSDesignPattern.Queries.SeriesQueries;


namespace Series.Api.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeriesController : ControllerBase
    {
        private readonly GetSeriesByIDQueryHandler _getSeriesByIDQueryHandler;
        private readonly GetSeriesQueryHandler _getSeriesQueryHandler;
        private readonly CreateSeriesCommandHandler _createSeriesCommandHandler;
        private readonly UpdateSeriesCommandHandler _updateSeriesCommandHandler;
        private readonly RemoveSeriesCommandHandler _removeSeriesCommandHandler;

        public SeriesController(GetSeriesByIDQueryHandler getSeriesByIDQueryHandler, GetSeriesQueryHandler getSeriesQueryHandler, CreateSeriesCommandHandler createSeriesCommandHandler, UpdateSeriesCommandHandler updateSeriesCommandHandler, RemoveSeriesCommandHandler removeSeriesCommandHandler)
        {
            _getSeriesByIDQueryHandler = getSeriesByIDQueryHandler;
            _getSeriesQueryHandler = getSeriesQueryHandler;
            _createSeriesCommandHandler = createSeriesCommandHandler;
            _updateSeriesCommandHandler = updateSeriesCommandHandler;
            _removeSeriesCommandHandler = removeSeriesCommandHandler;
        }

        [HttpGet]
        public async Task<IActionResult> SeriesList()
        {
            var values = await _getSeriesQueryHandler.Handle();
            return Ok(values);
        }
        [HttpPost]
        public async Task<IActionResult> CreateSeries(CreateSeriesCommand createSeriesCommand)
        {
            await _createSeriesCommandHandler.Handle(createSeriesCommand);
            return Ok("Dizi Başarıyla Eklendi..");
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteSeries(int id)
        {
            await _removeSeriesCommandHandler.Handle(new RemoveSeriesCommand(id));
            return Ok("Dizi Başarıyla Silindi..");
        }
        [HttpGet("GetMovile")]
        public async Task<IActionResult> GetSeries(int id)
        {
            var values = await _getSeriesByIDQueryHandler.Handle(new GetSeriesByIDQuery(id));
            return Ok(values);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateSeries(UpdateSeriesCommand updateSeriesCommand)
        {
            await _updateSeriesCommandHandler.Handle(updateSeriesCommand);
            return Ok("Dizi Başarıyla Güncellendi..");
        }
    }
}
