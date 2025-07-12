using Microsoft.AspNetCore.Mvc;
using MovieApi.Application.Features.QCRSDesignPattern.Commands.UserRegisterCommands;
using MovieApi.Application.Features.QCRSDesignPattern.Handlers.UserRegisterHandlers;

namespace Movie.Api.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistersController : ControllerBase
    {
        private readonly CreateUserRegisterCommandHandler _createUserRegisterCommandHandler;

        public RegistersController(CreateUserRegisterCommandHandler createUserRegisterCommandHandler)
        {
            _createUserRegisterCommandHandler = createUserRegisterCommandHandler;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUserRegister(CreateUserRegisterCommand command)
        {
            await _createUserRegisterCommandHandler.Handle(command);
            return Ok("Kullanıcı Başarıyla Eklendi");
        }
    }
}
