using Metete.Api.Features.Nacionalidades.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Metete.Api.Features.Nacionalidades
{
    [Route("api/[controller]")]
    [ApiController]
    public class NacionalidadesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public NacionalidadesController(IMediator mediator) => _mediator = mediator;

        /// <summary>
        /// Permite obtener todas las nacionalidades
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("dropdown-list")]
        public async Task<IActionResult> GetAllNacionalidadesForDropDownList()
        {
            List<GetAllNacionalidadesForDropDownList.NacionalidadDto> response = await _mediator.Send(new GetAllNacionalidadesForDropDownList.Query());
            return Ok(response);
        }
    }
}