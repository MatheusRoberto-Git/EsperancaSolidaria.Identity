using EsperancaSolidaria.Identity.Application.UseCases.Login.DoLogin;
using EsperancaSolidaria.Identity.Communication.Requests;
using EsperancaSolidaria.Identity.Communication.Responses;
using Microsoft.AspNetCore.Mvc;

namespace EsperancaSolidaria.Identity.API.Controllers
{    
    public class LoginController : BaseController
    {
        [HttpPost]
        [ProducesResponseType(typeof(ResponseRegisteredUserJson), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromServices] IDoLoginUseCase useCase, [FromBody] RequestLoginJson request)
        {
            var response = await useCase.Execute(request);
            
            return Ok(response);
        }
    }
}