using AutoMapper;
using MediatR;
using Metete.Api.Data;
using Metete.Api.Infraestructure.Exceptions;
using Metete.Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Security.Claims;

namespace Metete.Api.Features.Usuarios.Queries
{
    public class GetMyHorarios
    {
        public class Query : IRequest<List<HorarioDto>>
        {
        }

        public class Handler : IRequestHandler<Query, List<HorarioDto>>
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

            public async Task<List<HorarioDto>> Handle(Query query, CancellationToken cancellationToken)
            {
                int contextUsuarioId = int.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);


                Usuario? usuario = await _context.Usuarios
                    .Include(x => x.UsuarioHorarios)
                    .FirstOrDefaultAsync(x => x.Id == contextUsuarioId, cancellationToken);

                if (usuario == null)
                {
                    throw new EntityNotFoundException(nameof(Usuario), contextUsuarioId);
                }

                List<HorarioDto> result = usuario.UsuarioHorarios.Select(x => new HorarioDto
                {
                    Id = x.Id,
                    DiaDeLaSemana = x.DiaDeLaSemana,
                    HorarioInicio = x.HorarioInicio,
                    HorarioTermino = x.HorarioTermino,
                }).ToList();

                return result;
            }
        }

        public class HorarioDto
        {
            public int Id { get; set; }

            public int DiaDeLaSemana { get; set; }
            
            public TimeSpan HorarioInicio { get; set; }

            public TimeSpan HorarioTermino { get; set; }

            public string NombreDiaDeLaSemana { get { return new CultureInfo("es-CL").DateTimeFormat.DayNames[DiaDeLaSemana]; } }
        }
    }
}