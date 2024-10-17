using MediatR;
using Metete.Api.Enums;
using Metete.Api.Features.Eventos.Commands;
using Metete.Api.Features.Notificaciones.Commands;
using Metete.Api.Features.Notificaciones.Queries;
using Metete.Api.Features.Notificacions.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Metete.Api.Features.Notificaciones
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificacionesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public NotificacionesController(IMediator mediator) => _mediator = mediator;

        /// <summary>
        /// Permite obtener todas las notificaciones del usuario en contexto
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        public async Task<IActionResult> GetMyNotificaciones()
        {
            List<GetMyNotificaciones.NotificacionDto> response = await _mediator.Send(new GetMyNotificaciones.Query());
            return Ok(response);
        }

        /// <summary>
        /// Permite obtener la cantidad de notificaciones no leídas del usuario en contexto
        /// </summary>
        /// <returns></returns>
        [HttpGet("count-unreaded")]
        public async Task<IActionResult> CountUnreadedNotificacionesByUsuario()
        {
            int response = await _mediator.Send(new CountUnreadedNotificacionesByUsuario.Query());
            return Ok(response);
        }

        /// <summary>
        /// Permite marcar una notificación específica como leída
        /// </summary>
        /// <returns></returns>        
        [HttpPut("{id}/read")]
        public async Task<IActionResult> MarkNotificacionAsRead(int id)
        {
            await _mediator.Send(new MarkNotificacionAsRead.Command { Id = id });
            return NoContent();
        }

        /// <summary>
        /// Permite eliminar una notificación específica
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotification(int id)
        {
            await _mediator.Send(new DeleteNotification.Command { Id = id });
            return NoContent();
        }        

        /// <summary>
        /// Permite obtener todas las notificaciones push pendientes de envío (uso exclusivo para worker)
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("pending-push")]
        public async Task<IActionResult> GetPendingPushNotificaciones()
        {
            List<GetPendingPushNotificaciones.NotificacionDto> response = await _mediator.Send(new GetPendingPushNotificaciones.Query());
            return Ok(response);
        }

        /// <summary>
        /// Permite marcar una notificación específica como enviada (uso exclusivo para worker)
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPut("{id}/mark-enviada")]
        public async Task<IActionResult> MarkNotificacionAsEnviada(int id)
        {
            await _mediator.Send(new UpdateNotificacionStatus.Command { Id = id, IdEstadoNotificacion = (int)EstadoNotificacion.Enviada });
            return NoContent();
        }

        /// <summary>
        /// Permite marcar una notificación específica como fallida (uso exclusivo para worker)
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPut("{id}/mark-fallida")]
        public async Task<IActionResult> MarkNotificacionAsFallida(int id)
        {
            await _mediator.Send(new UpdateNotificacionStatus.Command { Id = id, IdEstadoNotificacion = (int)EstadoNotificacion.Fallida });
            return NoContent();
        }       
    }
}