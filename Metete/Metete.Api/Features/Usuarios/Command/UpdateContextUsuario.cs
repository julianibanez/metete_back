using AutoMapper;
using MediatR;
using Metete.Api.Data;
using Metete.Api.Infraestructure.Exceptions;
using Metete.Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Metete.Api.Features.Usuarios.Command
{
    public class UpdateContextUsuario
    {
        public class Command : IRequest<Unit>
        {
            public string Nombre { get; set; } = null!;

            public string ApellidoPaterno { get; set; } = null!;

            public string ApellidoMaterno { get; set; } = null!;

            public DateTime FechaNacimiento { get; set; }

            public int IdTipoGenero { get; set; }

            public int CodigoPais { get; set; }

            public string Telefono { get; set; } = null!;

            public int IdNacionalidad { get; set; }

            public string Direccion { get; set; } = null!;

            public int IdComuna { get; set; }

            public decimal Latitud { get; set; }

            public decimal Longitud { get; set; }

            public int? KmBusqueda { get; set; }
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly MeteteContext _context;
            private readonly IMapper _mapper;
            private readonly IConfiguration _configuration;
            private readonly IHttpContextAccessor _httpContextAccessor;

            public Handler(MeteteContext context, IMapper mapper, IConfiguration configuration,  IHttpContextAccessor httpContextAccessor)
            {
                _context = context;
                _mapper = mapper;
                _configuration = configuration;
                _httpContextAccessor = httpContextAccessor;
            }

            public async Task<Unit> Handle(Command command, CancellationToken cancellationToken)
            {
                int contextUsuarioId = int.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

                Usuario? usuario = await _context.Usuarios.FirstOrDefaultAsync(x => x.Id == contextUsuarioId, cancellationToken);

                if (usuario == null)
                {
                    throw new EntityNotFoundException(nameof(Usuario), contextUsuarioId);
                }

                usuario = _mapper.Map(command, usuario);

                _context.Usuarios.Update(usuario);

                await _context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}