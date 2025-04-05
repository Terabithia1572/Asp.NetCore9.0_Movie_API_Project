using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieApi.Application.Features.MediatorDesignPattern.Commands.CastCommands;
using MovieApi.Application.Features.MediatorDesignPattern.Queries.CastQueries;

namespace Movie.Api.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CastsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CastsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task< IActionResult> CastList()
        {
            var values =await _mediator.Send(new GetCastQuery());
            return Ok(values);
        }
        [HttpPost]
        public async Task<IActionResult> CreateCast(CreateCastCommand createCastCommand)
        {
            await _mediator.Send(createCastCommand);
            return Ok("Ekleme İşlemi Başarılı..");

        }
        [HttpDelete]
        public async Task<IActionResult> DeleteCast(int id)
        {
           await _mediator.Send(new RemoveCastCommand(id));
            return Ok("Silme İşlemi Başarılı..");
        }
        [HttpGet("GetCastByID")]
        public async Task<IActionResult> GetCastByID(int id)
        {
            var values = await _mediator.Send(new GetCastByIDQuery(id));
            return Ok(values);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateCast(UpdateCastCommand updateCastCommand)
        {
            await _mediator.Send(updateCastCommand);
            return Ok("Güncelleme İşlemi Başarılı..");
        }
    }
}
