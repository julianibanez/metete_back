using AutoMapper;
using Metete.Api.Features.MetodoPagos.Queries;
using Metete.Api.Models;

namespace Metete.Api.Features.MetodoPagos
{
    public class MetodoPagosMappingProfile: Profile
    {
        public MetodoPagosMappingProfile()
        {
            CreateMap<MetodoPago, GetAllMetodoPagosForDropDownList.MetodoPagoDto>();
        }
    }
}
