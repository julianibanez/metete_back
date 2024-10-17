using MediatR;
using Metete.Api.Features.GoogleMaps.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Metete.Api.Features.MetodoPagos
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoogleMapsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GoogleMapsController(IMediator mediator) => _mediator = mediator;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>        
        [HttpGet("place/autocomplete")]
        public async Task<IActionResult> GetPlacePredictions([FromQuery] GetPlacePredictions.Query query)
        {
            var response = await _mediator.Send(query);
            return Ok(response);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>        
        [HttpGet("geocode")]
        public async Task<IActionResult> GetGeocode([FromQuery] GetGeocode.Query query)
        {
            var response = await _mediator.Send(query);
            return Ok(response);
        }
    }
}