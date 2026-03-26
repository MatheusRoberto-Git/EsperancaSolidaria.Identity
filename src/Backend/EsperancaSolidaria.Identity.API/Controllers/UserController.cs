using EsperancaSolidaria.Identity.API.Attributes;
using EsperancaSolidaria.Identity.Application.UseCases.User.Register;
using EsperancaSolidaria.Identity.Application.UseCases.User.UpdateRole;
using EsperancaSolidaria.Identity.Communication.Requests;
using EsperancaSolidaria.Identity.Communication.Responses;
using EsperancaSolidaria.Identity.Domain.Enums;
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

        [HttpPatch]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
        [AuthenticatedUser]
        [AuthorizeRole(UserRole.GestorONG)]
        public async Task<IActionResult> UpdateRole([FromServices] IUpdateRoleUserUseCase useCase, [FromBody] RequestUpdateRoleUserJson request)
        {
            await useCase.Execute(request);

            return NoContent();
        }
    }
}