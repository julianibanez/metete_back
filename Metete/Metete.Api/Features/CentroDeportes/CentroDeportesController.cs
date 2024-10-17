using Metete.Api.Features.CentroDeportes.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Metete.Api.Features.CentroDeportes
{
    [Route("api/[controller]")]
    [ApiController]
    public class CentroDeportesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CentroDeportesController(IMediator mediator) => _mediator = mediator;

        /// <summary>
        /// Permite obtener todos los centros de deporte que coincidan aproximádamente con una ubicación
        /// </summary>
        /// <param name="ubicacion"></param>
        /// <returns></returns>
        [HttpGet("{ubicacion}")]
        public async Task<IActionResult> GetAllCentroDeporteForDropDownList(string ubicacion)
        {
            List<GetCentroDeporteByUbicacion.CentroDeporteDto> response = await _mediator.Send(new GetCentroDeporteByUbicacion.Query { Ubicacion = ubicacion });
            return Ok(response);
        }
    }
}