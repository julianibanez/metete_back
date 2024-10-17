using Metete.Api.Features.Paises.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Metete.Api.Features.Paises
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaisesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PaisesController(IMediator mediator) => _mediator = mediator;

        /// <summary>
        /// Permite obtener todos los países
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("dropdown-list")]
        public async Task<IActionResult> GetAllPaisesForDropDownList()
        {
            List<GetAllPaisesForDropDownList.PaisDto> response = await _mediator.Send(new GetAllPaisesForDropDownList.Query());
            return Ok(response);
        }
    }
}