using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Metete.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace Metete.Api.Features.Eventos.Queries
{
    public class GetEnrollmentRequestsByEvento
    {
        public class Query : IRequest<List<RequestDto>>
        {
            public int Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, List<RequestDto>>
        {
            private readonly MeteteContext _context;
            private readonly IMapper _mapper;

            public Handler(MeteteContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<List<RequestDto>> Handle(Query query, CancellationToken cancellationToken)
            {
                // TODO: Agregar campo fecha de enrolamiento para poder ordenar por antigüedad

                List<RequestDto> eventos = await _context.UsuarioEventos
                    .Include(x => x.Usuario)
                    .AsNoTracking()
                    .Where(x => x.IdEvento == query.Id)
                    .ProjectTo<RequestDto>(_mapper.ConfigurationProvider)
                    .ToListAsync();

                return eventos;
            }
        }

        public class RequestDto
        {
            public int Id { get; set; }
            public string NombreUsuario { get; set; } = null!;
            public string Posicion { get; set; } = null!;
            public bool? Aprobado { get; set; }
            public string? Foto { get; set; }
        }
    }
}
