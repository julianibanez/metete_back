using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Metete.Api.Data;
using Metete.Api.Enums;
using Microsoft.EntityFrameworkCore;

namespace Metete.Api.Features.Notificacions.Queries
{
    public class GetPendingPushNotificaciones
    {
        public class Query : IRequest<List<NotificacionDto>>
        {
        }

        public class Handler : IRequestHandler<Query, List<NotificacionDto>>
        {
            private readonly MeteteContext _context;
            private readonly IMapper _mapper;

            public Handler(MeteteContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<List<NotificacionDto>> Handle(Query query, CancellationToken cancellationToken)
            {
                List<NotificacionDto> notificaciones = await _context.Notificaciones
                    .Include(x => x.Usuario)
                    .Include(x => x.TipoNotificacion)
                    .AsNoTracking()
                    .Where(x => x.IdEstadoNotificacion == (int)EstadoNotificacion.Pendiente
                        && x.Usuario.FcmToken != null)
                    .OrderBy(x => x.FechaCreacion)
                    .ProjectTo<NotificacionDto>(_mapper.ConfigurationProvider)
                    .ToListAsync();

                return notificaciones;
            }
        }

        public class NotificacionDto
        {
            public int Id { get; set; }

            public string FcmToken { get; set; } = null!;

            public string Titulo { get; set; } = null!;

            public string Mensaje { get; set; } = null!;

            public int? IdEvento { get; set; }

            public int? IdTipoNotificacion { get; set; }
        }
    }
}
