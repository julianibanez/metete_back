using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Metete.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace Metete.Api.Features.MetodoPagos.Queries
{
    public class GetAllMetodoPagosForDropDownList
    {
        public class Query : IRequest<List<MetodoPagoDto>>
        {
        }

        public class Handler : IRequestHandler<Query, List<MetodoPagoDto>>
        {
            private readonly MeteteContext _context;
            private readonly IMapper _mapper;

            public Handler(MeteteContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<List<MetodoPagoDto>> Handle(Query query, CancellationToken cancellationToken)
            {
                List<MetodoPagoDto> metodoPagos = await _context.MetodoPagos.AsNoTracking()
                     .OrderBy(x => x.Nombre)
                     .ProjectTo<MetodoPagoDto>(_mapper.ConfigurationProvider)
                     .ToListAsync();

                return metodoPagos;
            }
        }

        public class MetodoPagoDto
        {
            public int Id { get; set; }

            public string Nombre { get; set; } = null!;
        }
    }
}
