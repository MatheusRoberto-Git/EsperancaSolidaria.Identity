using EsperancaSolidaria.Identity.Application.UseCases.User.Register;
using EsperancaSolidaria.Identity.Communication.Requests;
using EsperancaSolidaria.Identity.Communication.Responses;
using Microsoft.AspNetCore.Mvc;

namespace EsperancaSolidaria.Identity.API.Controllers
{    
    public class UserController : BaseController
    {
        [HttpPost]
        [ProducesResponseType(typeof(ResponseRegisteredUserJson), StatusCodes.Status201Created)]
        public async Task<IActionResult> Register([FromServices] IRegisterUserUseCase useCase, [FromBody] RequestRegisterUserJson request)
        {
            var result = await useCase.Execute(request);

            return Created(string.Empty, result);
        }
    }
}