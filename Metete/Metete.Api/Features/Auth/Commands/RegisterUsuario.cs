using AutoMapper;
using MediatR;
using Metete.Api.Data;
using Metete.Api.Infraestructure.Exceptions;
using Metete.Api.Models;
using Microsoft.EntityFrameworkCore;
using BC = BCrypt.Net.BCrypt;

namespace Metete.Api.Features.Auth.Commands
{
    public class RegisterUsuario
    {
        public class Command : IRequest<Unit>
        {
            public string Username { get; set; } = null!;

            public string Password { get; set; } = null!;

            public string Nombre { get; set; } = null!;

            public string ApellidoPaterno { get; set; } = null!;

            public string ApellidoMaterno { get; set; } = null!;

            public DateTime FechaNacimiento { get; set; }

            public int IdTipoGenero { get; set; }

            public int CodigoPais { get; set; }

            public string Telefono { get; set; } = null!;

            public int IdPaisResidencia { get; set; }

            public int IdNacionalidad { get; set; }

            public string Direccion { get; set; } = null!;

            public int IdComuna { get; set; }

            public decimal Latitud { get; set; }

            public decimal Longitud { get; set; }

            public int IdTipoMembresia { get; set; }          
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly MeteteContext _context;
            private readonly IMapper _mapper;
            private readonly IConfiguration _configuration;

            public Handler(MeteteContext context, IMapper mapper, IConfiguration configuration)
            {
                _context = context;
                _mapper = mapper;
                _configuration = configuration;
            }

            public async Task<Unit> Handle(Command command, CancellationToken cancellationToken)
            {
                bool isTaken = await _context.Usuarios.AnyAsync(x => x.Username == command.Username, cancellationToken);

                if (isTaken)
                {
                    throw new BusinessRuleException("El nombre de usuario ya está en uso");
                }

                Usuario usuario = _mapper.Map<Usuario>(command);
                usuario.Password = BC.HashPassword(command.Password);
                usuario.IdTipoMembresia = 1; // TODO: Enviar desde el front
                usuario.Activo = true;

                _context.Usuarios.Add(usuario);

                await _context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }       
    }
}
