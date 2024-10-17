using Metete.Api.Features.TipoGeneros.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Metete.Api.Features.TipoGeneros
{
    [Route("api/[controller]")]
    [ApiController]
    public class TipoGenerosController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TipoGenerosController(IMediator mediator) => _mediator = mediator;

        /// <summary>
        /// Permite obtener todos los tipos de género
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("dropdown-list")]
        public async Task<IActionResult> GetAllTipoGenerosForDropDownList()
        {
            List<GetAllTipoGenerosForDropDownList.TipoGeneroDto> response = await _mediator.Send(new GetAllTipoGenerosForDropDownList.Query());
            return Ok(response);
        }
    }
}