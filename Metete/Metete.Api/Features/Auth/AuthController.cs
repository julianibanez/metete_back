using MediatR;
using Metete.Api.Features.Auth.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Metete.Api.Features.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration;

        public AuthController(IMediator mediator, IConfiguration configuration)
        {
            _mediator =mediator;
            _configuration = configuration;
        }

        /// <summary>
        /// Permite iniciar sesión a un usuario registrado 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("sign-in")]
        public async Task<IActionResult> AuthenticateUsuario(AuthenticateUsuario.Command command)
        {
            AuthenticateUsuario.AuthDataDto response = await _mediator.Send(command);

            return Ok(response);
        }

        /// <summary>
        /// Permite determinar si un username se encuentra disponible  
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("username-availability")]
        public async Task<IActionResult> CheckUsernameAvailability(CheckUsernameAvailability.Command command)
        {
            bool response = await _mediator.Send(command);

            return Ok(response);
        }

        /// <summary>
        /// Permite realizar el registro de un usuario nuevo
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("sign-up")]
        public async Task<IActionResult> RegisterUsuario(RegisterUsuario.Command command)
        {
            await _mediator.Send(command);

            return NoContent();
        }

        /// <summary>
        /// Permite recuperar la contraseña de un usuario específico
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("recovery")]
        [HttpPost]
        public async Task<IActionResult> RecoveryPasswordUsuario(RecoverPasswordUsuario.Command command)
        {
            await _mediator.Send(command);

            return NoContent();
        }

        /// <summary>
        /// Permite resetear la contraseña de un usuario específico
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("reset")]
        [HttpPost]
        public async Task<IActionResult> RecoveryPasswordUsuario(ResetPasswordUsuario.Command command)
        {
            await _mediator.Send(command);

            return NoContent();
        }

        /// <summary>
        /// Permite validar el código de recuperación de contraseña de un usuario específico
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("recovery-code-validation")]
        [HttpPost]
        public async Task<IActionResult> ValidateRecoveryCode(ValidateRecoveryCode.Command command)
        {
            bool reponse = await _mediator.Send(command);

            return Ok(reponse);
        }

        /// <summary>
        /// Permite obtener los términos y condiciones para registro y uso de la app
        /// </summary>       
        /// <returns></returns>
        [AllowAnonymous]
        [Route("terms")]
        [HttpGet]
        public async Task<IActionResult> GetTermsAndCondictionsUrl()
        {
            string url = _configuration["Params:TermsConditionsUrl"]!;

            var response = new 
            {
                url
            };

            return Ok(response);
        }

        /// <summary>
        /// Permite obtener las política de privaciudad de la app
        /// </summary>       
        /// <returns></returns>
        [AllowAnonymous]
        [Route("policy")]
        [HttpGet]
        public async Task<IActionResult> GetPrivacyPolicyUrl()
        {
            string url = _configuration["Params:PrivacyPolicyUrl"]!;

            var response = new
            {
                url
            };

            return Ok(response);
        }

        /// <summary>
        /// Permite refrescar la sesión de un usuario registrado 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken(RefreshToken.Command command)
        {
            var response = await _mediator.Send(command);

            return Ok(response);
        }
    }
}
