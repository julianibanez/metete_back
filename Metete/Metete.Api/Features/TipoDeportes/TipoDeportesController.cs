using Metete.Api.Features.TipoDeportes.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Metete.Api.Features.TipoDeportes
{
    [Route("api/[controller]")]
    [ApiController]
    public class TipoDeportesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TipoDeportesController(IMediator mediator) => _mediator = mediator;

        /// <summary>
        /// Permite obtener todos los tipos de deporte
        /// </summary>
        /// <returns></returns>
        [HttpGet("dropdown-list")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllTipoDeportesForDropDownList()
        {
            List<GetAllTipoDeportesForDropDownList.TipoDeporteDto> response = await _mediator.Send(new GetAllTipoDeportesForDropDownList.Query());
            return Ok(response);
        }
    }
}