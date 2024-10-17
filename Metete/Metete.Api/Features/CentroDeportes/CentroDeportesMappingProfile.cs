using AutoMapper;
using Metete.Api.Features.CentroDeportes.Queries;
using Metete.Api.Models;

namespace Metete.Api.Features.CentroDeportes
{
    public class CentroDeportes : Profile
    {
        public CentroDeportes()
        {
            CreateMap<CentroDeporte, GetCentroDeporteByUbicacion.CentroDeporteDto>();
        }
    }
}
