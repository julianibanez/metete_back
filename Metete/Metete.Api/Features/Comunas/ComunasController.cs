using Metete.Api.Features.Comunas.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Metete.Api.Features.Comunas
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComunasController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ComunasController(IMediator mediator) => _mediator = mediator;

        /// <summary>
        /// Permite obtener todas las comunas
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("dropdown-list")]
        public async Task<IActionResult> GetAllComunasForDropDownList()
        {
            List<GetAllComunasForDropDownList.ComunaDto> response = await _mediator.Send(new GetAllComunasForDropDownList.Query());
            return Ok(response);
        }
    }
}