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
        public IActionResult CastList()
        {
            var values = _mediator.Send(new GetCastQuery());
            return Ok(values);
        }
        [HttpPost]
        public IActionResult CreateCast(CreateCastCommand createCastCommand)
        {
            _mediator.Send(createCastCommand);
            return Ok("Ekleme İşlemi Başarılı..");

        }
        [HttpDelete]
        public IActionResult DeleteCast(int id)
        {
            _mediator.Send(new RemoveCastCommand(id));
            return Ok("Silme İşlemi Başarılı..");
        }
        [HttpGet("GetCastByID")]
        public IActionResult GetCastByID(int id)
        {
            var values = _mediator.Send(new GetCastByIDQuery(id));
            return Ok(values);
        }
        [HttpPut]
        public IActionResult UpdateCast(UpdateCastCommand updateCastCommand)
        {
            _mediator.Send(updateCastCommand);
            return Ok("Güncelleme İşlemi Başarılı..");
        }
    }
}
