using AutoMapper;
using MediatR;
using Metete.Api.Data;
using Metete.Api.Infraestructure.Exceptions;
using Metete.Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Metete.Api.Features.Usuarios.Queries
{
    public class GetContextUsuario
    {
        public class Query : IRequest<UsuarioDto>
        {          
        }

        public class Handler : IRequestHandler<Query, UsuarioDto>
        {
            private readonly MeteteContext _context;
            private readonly IMapper _mapper;
            private readonly IHttpContextAccessor _httpContextAccessor;

            public Handler(MeteteContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor)
            {
                _context = context;
                _mapper = mapper;
                _httpContextAccessor = httpContextAccessor;
            }

            public async Task<UsuarioDto> Handle(Query query, CancellationToken cancellationToken)
            {
                int contextUsuarioId = int.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

                Usuario? usuario = await _context.Usuarios
                    .Include(x => x.Nacionalidad)
                    .Include(x => x.Comuna)
                    .FirstOrDefaultAsync(x => x.Id == contextUsuarioId, cancellationToken);

                if (usuario == null)
                {
                    throw new EntityNotFoundException(nameof(Usuario), contextUsuarioId);
                }

                UsuarioDto result = _mapper.Map<UsuarioDto>(usuario);

                return result;
            }
        }

        public class UsuarioDto
        {
            public int Id { get; set; }

            public string Username { get; set; } = null!;

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

            public string NombreNacionalidad { get; set; } = null!;

            public string NombreComuna { get; set; } = null!;

            public int? KmBusqueda { get; set; }
        }
    }
}