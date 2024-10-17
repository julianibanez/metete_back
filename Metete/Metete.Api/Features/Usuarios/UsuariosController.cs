using MediatR;
using Metete.Api.Features.Usuarios.Command;
using Metete.Api.Features.Usuarios.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Metete.Api.Features.TipoDeportes
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsuariosController(IMediator mediator) => _mediator = mediator;

        /// <summary>
        /// Registra el token FCM para realizar las notificaciones push
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("fcm-token")]
        public async Task<IActionResult> RegisterFcmToken(RegisterFcmToken.Command command)
        {
            await _mediator.Send(command);

            return NoContent();
        }

        /// <summary>
        /// Permite obtener los datos actualizables del usuario en contexto
        /// </summary>
        /// <returns></returns>
        [HttpGet("context-usuario")]
        public async Task<IActionResult> GetContextUsuario()
        {
            GetContextUsuario.UsuarioDto usuario = await _mediator.Send(new GetContextUsuario.Query());

            return Ok(usuario);
        }

        /// <summary>
        /// Permite actualizar los datos del usuario en contexto
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("context-usuario")]
        public async Task<IActionResult> UpdateContextUsuario(UpdateContextUsuario.Command command)
        {
            await _mediator.Send(command);

            return NoContent();
        }

        /// <summary>
        /// Permite obtener los deportes del usuario en contexto
        /// </summary>
        /// <returns></returns>
        [HttpGet("mis-deportes")]
        public async Task<IActionResult> GetMyDeportes()
        {
            List<int> tipoDeportes = await _mediator.Send(new GetMyDeportes.Query());

            return Ok(tipoDeportes);
        }

        /// <summary>
        /// Permite actualizar los deportes del usuario en contexto
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("mis-deportes")]
        public async Task<IActionResult> UpdateMyDeportes(UpdateMyDeportes.Command command)
        {
            await _mediator.Send(command);

            return NoContent();
        }

        /// <summary>
        /// Permite obtener los horarios del usuario en contexto
        /// </summary>
        /// <returns></returns>
        [HttpGet("mis-horarios")]        
        public async Task<IActionResult> GetMyHorarios()
        {
            List<GetMyHorarios.HorarioDto> tipoDeportes = await _mediator.Send(new GetMyHorarios.Query());

            return Ok(tipoDeportes);
        }

        /// <summary>
        /// Permite actualizar los horarios del usuario en contexto
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("mis-horarios")]
        public async Task<IActionResult> UpdateMyHorarios(UpdateMyHorarios.Command command)
        {
            await _mediator.Send(command);

            return NoContent();
        }

        /// <summary>
        /// Permite eliminar un horario específico del usuario en contexto
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("mis-horarios/{id}")]
        public async Task<IActionResult> DeleteHorario(int id)
        {
            await _mediator.Send(new DeleteHorario.Command { Id = id });

            return NoContent();
        }

        /// <summary>
        /// Permite obtener el record de eventos del usuario en contexto
        /// </summary>
        /// <returns></returns>
        [HttpGet("mi-record")]
        public async Task<IActionResult> GetMyRecord()
        {
            GetMyRecord.RecordDto usuario = await _mediator.Send(new GetMyRecord.Query());

            return Ok(usuario);
        }

        /// <summary>
        /// Permite obtener datos de usuarios específicos para el chat
        /// </summary>
        /// <returns></returns>
        [HttpGet("chat-data")]
        public async Task<IActionResult> GetContextUsuario([FromQuery] GetUsuariosForChat.Query query)
        {
            List<GetUsuariosForChat.UsuarioDto> usuarios = await _mediator.Send(query);

            return Ok(usuarios);
        }

        /// <summary>
        /// Permite eliminar el usuario en contexto
        /// </summary>
        /// <returns></returns>
        [HttpDelete("account")]
        public async Task<IActionResult> DeleteMyAccount()
        {
            await _mediator.Send(new DeleteMyAccount.Command());

            return NoContent();
        }

        [HttpPost("foto")]
        public async Task<IActionResult> UploadFoto([FromForm] UploadFoto.Command command)
        {
            var foto = await _mediator.Send(command);
            return Ok(foto);
        }

        /// <summary>
        /// Permite obtener los horarios del usuario en contexto
        /// </summary>
        /// <returns></returns>
        [HttpGet("mis-comunas")]        
        public async Task<IActionResult> GetMyComunas()
        {
            var comunas = await _mediator.Send(new GetMyComunas.Query());

            return Ok(comunas);
        }

        /// <summary>
        /// Permite actualizar las comunas del usuario en contexto
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("mis-comunas")]
        public async Task<IActionResult> UpdateMyComunas(UpdateMyComunas.Command command)
        {
            await _mediator.Send(command);

            return NoContent();
        }
    }
}