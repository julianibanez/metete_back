using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Metete.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace Metete.Api.Features.Comunas.Queries
{
    public class GetAllComunasForDropDownList
    {
        public class Query : IRequest<List<ComunaDto>>
        {
        }

        public class Handler : IRequestHandler<Query, List<ComunaDto>>
        {
            private readonly MeteteContext _context;
            private readonly IMapper _mapper;

            public Handler(MeteteContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<List<ComunaDto>> Handle(Query query, CancellationToken cancellationToken)
            {
                List<ComunaDto> comunas = await _context.Comunas.AsNoTracking()
                     .OrderBy(x => x.Nombre)
                     .ProjectTo<ComunaDto>(_mapper.ConfigurationProvider)
                     .ToListAsync();

                return comunas;
            }
        }

        public class ComunaDto
        {
            public int Id { get; set; }

            public string Nombre { get; set; } = null!;

            public decimal Latitud { get; set; }

            public decimal Longitud { get; set; }
        }
    }
}
