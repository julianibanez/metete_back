using AutoMapper;
using MediatR;
using Metete.Api.Data;
using Metete.Api.Infraestructure.Constants;
using Metete.Api.Infraestructure.Exceptions;
using Metete.Api.Models;
using Metete.Api.Services;
using Microsoft.EntityFrameworkCore;
using BC = BCrypt.Net.BCrypt;

namespace Metete.Api.Features.Auth.Commands
{
    public class AuthenticateUsuario
    {
        public class Command : IRequest<AuthDataDto>
        {
            public string Username { get; set; } = null!;
            public string Password { get; set; } = null!;
        }

        public class Handler : IRequestHandler<Command, AuthDataDto>
        {
            private readonly MeteteContext _context;
            private readonly IMapper _mapper;
            private readonly IConfiguration _configuration;
            private readonly ITokenService _tokenService;

            public Handler(MeteteContext context, IMapper mapper, IConfiguration configuration, ITokenService tokenService)
            {
                _context = context;
                _mapper = mapper;
                _configuration = configuration;
                _tokenService = tokenService;
            }

            public async Task<AuthDataDto> Handle(Command command, CancellationToken cancellationToken)
            {
                Usuario? usuario = await _context.Usuarios
                    .FirstOrDefaultAsync(x => x.Username == command.Username, cancellationToken);

                if (usuario == null)
                {
                    throw new LoginFailedException(Feedback.USUARIO_NOT_FOUND);
                }

                if (!usuario.Activo)
                {
                    throw new LoginFailedException(Feedback.USUARIO_NOT_ACTIVE);
                }

                if (!BC.Verify(command.Password, usuario.Password))
                {
                    throw new LoginFailedException("Contraseña no válida");
                }

                string accessToken = _tokenService.GenerateAccessToken(usuario);
                string refreshToken = _tokenService.GenerateRefreshToken();

                usuario.RefreshToken = refreshToken;
                usuario.RefreshTokenExpiryTime = DateTime.UtcNow.AddMinutes(int.Parse(_configuration["RefreshToken:ExpiresIn"]!));

                _context.Usuarios.Update(usuario);
                await _context.SaveChangesAsync(cancellationToken);

                AuthDataDto authData = new()
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                };

                return authData;
            }
        }

        public class AuthDataDto
        {
            public string AccessToken { get; set; } = null!;
            public string RefreshToken { get; set; } = null!;
        }
    }
}
