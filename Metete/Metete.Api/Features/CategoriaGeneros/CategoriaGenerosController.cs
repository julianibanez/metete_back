using Metete.Api.Features.CategoriaGeneros.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Metete.Api.Features.CategoriaGeneros
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaGeneros : ControllerBase
    {
        private readonly IMediator _mediator;

        public CategoriaGeneros(IMediator mediator) => _mediator = mediator;

        /// <summary>
        /// Permite obtener todas las categorías de género
        /// </summary>
        /// <returns></returns>
        [HttpGet("dropdown-list")]
        public async Task<IActionResult> GetAllCategoriaGenerosForDropDownList()
        {
            List<GetAllCategoriaGenerosForDropDownList.CategoriaGeneroDto> response = await _mediator.Send(new GetAllCategoriaGenerosForDropDownList.Query());
            return Ok(response);
        }
    }
}