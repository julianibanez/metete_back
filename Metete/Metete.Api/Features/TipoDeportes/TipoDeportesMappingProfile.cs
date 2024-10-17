using AutoMapper;
using Metete.Api.Features.TipoDeportes.Queries;
using Metete.Api.Models;

namespace Metete.Api.Features.TipoDeportes
{
    public class TipoDeportesMappingProfile: Profile
    {
        public TipoDeportesMappingProfile()
        {
            CreateMap<TipoDeporte, GetAllTipoDeportesForDropDownList.TipoDeporteDto>();
        }
    }
}
