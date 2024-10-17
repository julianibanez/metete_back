using Metete.Api.Features.MetodoPagos.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Metete.Api.Features.MetodoPagos
{
    [Route("api/[controller]")]
    [ApiController]
    public class MetodoPagosController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MetodoPagosController(IMediator mediator) => _mediator = mediator;

        /// <summary>
        /// Permite obtener todos los métodos de pago
        /// </summary>
        /// <returns></returns>
        [HttpGet("dropdown-list")]
        public async Task<IActionResult> GetAllMetodoPagosForDropDownList()
        {
            List<GetAllMetodoPagosForDropDownList.MetodoPagoDto> response = await _mediator.Send(new GetAllMetodoPagosForDropDownList.Query());
            return Ok(response);
        }
    }
}