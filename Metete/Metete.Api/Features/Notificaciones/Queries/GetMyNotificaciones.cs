using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Metete.Api.Data;
using Metete.Api.Enums;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Metete.Api.Features.Notificaciones.Queries
{
    public class GetMyNotificaciones
    {
        public class Query : IRequest<List<NotificacionDto>>
        {
        }

        public class Handler : IRequestHandler<Query, List<NotificacionDto>>
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

            public async Task<List<NotificacionDto>> Handle(Query query, CancellationToken cancellationToken)
            {
                int contextUsuarioId = int.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

                List<NotificacionDto> notificaciones = await _context.Notificaciones
                    .Include(x => x.Usuario)
                    .Include(x => x.TipoNotificacion)
                    .AsNoTracking()
                    .Where(x => x.IdUsuario == contextUsuarioId
                        && x.IdEstadoNotificacion == (int)EstadoNotificacion.Enviada)
                    .OrderByDescending(x => x.FechaCreacion)
                    .ProjectTo<NotificacionDto>(_mapper.ConfigurationProvider)
                    .ToListAsync();

                return notificaciones;
            }
        }

        public class NotificacionDto
        {
            public int Id { get; set; }

            public bool Leido { get; set; }

            public string Titulo { get; set; } = null!;

            public string Mensaje { get; set; } = null!;

            public DateTime FechaEnvio { get; set; }

            public int? IdEvento { get; set; }

            public int? IdTipoNotificacion { get; set; }
        }
    }
}