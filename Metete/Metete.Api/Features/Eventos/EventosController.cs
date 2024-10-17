using Metete.Api.Features.Eventos.Queries;
using MediatR;
using Metete.Api.Features.Eventos.Commands;
using Microsoft.AspNetCore.Mvc;
using Metete.Api.Features.Notificaciones.Commands;

namespace Metete.Api.Features.TipoDeportes
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventosController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EventosController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> GetAllEventos([FromQuery] GetAllEventos.Query query)
        {
            List<GetAllEventos.EventoDto> response = await _mediator.Send(query);
            return Ok(response);
        }

        /// <summary>
        /// Permite obtener el detalle de un evento específico
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}/detail")]
        public async Task<IActionResult> GetEventoForDetail(int id)
        {
            GetEventoForDetail.EventoDto response = await _mediator.Send(new GetEventoForDetail.Query { Id = id });
            return Ok(response);
        }

        /// <summary>
        /// Crea un nuevo evento
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateEvento(CreateEvento.Command command)
        {
            int id = await _mediator.Send(command);

            return StatusCode(StatusCodes.Status201Created, id);
        }

        /// <summary>
        /// Actualiza un evento existente
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> UpdateEvento(UpdateEvento.Command command)
        {
            await _mediator.Send(command);

            return NoContent();
        }

        /// <summary>
        /// Permite eliminar un evento específico
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvento(int id)
        {
            await _mediator.Send(new DeleteEvento.Command { Id = id });

            return NoContent();
        }

        /// <summary>
        /// Permite unirse a un evento específico
        /// </summary>
        /// <param name="id"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("{id}/enroll")]
        public async Task<IActionResult> EnrollEvento(int id, EnrollEvento.Command command)
        {
            await _mediator.Send(command);

            return NoContent();
        }

        /// <summary>
        /// Permite abandonar un evento específico
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}/unenroll")]
        public async Task<IActionResult> UnenrollEvento(int id)
        {
            await _mediator.Send(new UnenrollEvento.Command { Id = id });

            return NoContent();
        }

        /// <summary>
        /// Permite obtener las solicitudes de participación de un evento específico
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}/solicitudes")]
        public async Task<IActionResult> GetEnrollmentRequestsByEvento(int id)
        {
            List<GetEnrollmentRequestsByEvento.RequestDto> response = await _mediator.Send(new GetEnrollmentRequestsByEvento.Query { Id = id });
            return Ok(response);
        }

        /// <summary>
        /// Permite confirmar un solicitud de participación específica
        /// </summary>
        /// <param name="id"></param>
        /// <param name="idSolicitud"></param>
        /// <returns></returns>
        [HttpPut("{id}/solicitudes/{idSolicitud}/confirm")]
        public async Task<IActionResult> ConfirmEnrollment(int id, int idSolicitud)
        {
            await _mediator.Send(new ConfirmEnrollment.Command { Id = idSolicitud });

            return NoContent();
        }

        /// <summary>
        /// Permite rechazar un solicitud de participación específica
        /// </summary>
        /// <param name="id"></param>
        /// <param name="idSolicitud"></param>
        /// <returns></returns>
        [HttpPut("{id}/solicitudes/{idSolicitud}/reject")]
        public async Task<IActionResult> RejectEnrollment(int id, int idSolicitud)
        {
            await _mediator.Send(new RejectEnrollment.Command { Id = idSolicitud });

            return NoContent();
        }

        /// <summary>
        /// Permite obtener los top n eventos creados recientemente
        /// </summary>
        /// <returns></returns>
        [HttpGet("top-recent")]
        public async Task<IActionResult> GetTopRecentEventos()
        {
            List<GetTopRecentEventos.EventoDto> response = await _mediator.Send(new GetTopRecentEventos.Query());
            return Ok(response);
        }

        /// <summary>
        /// Crea una notificación de chat para todos los usuarios aprobados en un evento
        /// </summary>
        /// <returns></returns>
        [HttpPost("{id}/chat-notificacion")]
        public async Task<IActionResult> CreateChatNotificacion(int id)
        {
            await _mediator.Send(new CreateChatNotificacion.Command { Id = id });
            return NoContent();
        }
    }
}