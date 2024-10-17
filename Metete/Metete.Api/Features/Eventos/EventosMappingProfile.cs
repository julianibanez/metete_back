using AutoMapper;
using Metete.Api.Features.Eventos.Commands;
using Metete.Api.Features.Eventos.Queries;
using Metete.Api.Models;

namespace Metete.Api.Features.TipoDeportes
{
    public class EventosMappingProfile : Profile
    {
        public EventosMappingProfile()
        {
            // Queiries
            CreateMap<Evento, GetAllEventos.EventoDto>()
                .ForMember(dest => dest.NombreTipoDeporte, opt => opt.MapFrom(src => src.TipoDeporte.Nombre))
                .ForMember(dest => dest.NombreCentroDeporte, opt => opt.MapFrom(src =>
                    src.OtroCentroDeporte ? src.NombreCentroDeporte : src.CentroDeporte.Nombre))
                .ForMember(dest => dest.DireccionCentroDeporte, opt => opt.MapFrom(src =>
                    src.OtroCentroDeporte ? src.DireccionCentroDeporte : string.Join(", ", src.CentroDeporte.Direccion, src.CentroDeporte.Comuna.Nombre)))
                .ForMember(dest => dest.Latitud, opt => opt.MapFrom(src =>
                    src.OtroCentroDeporte ? src.LatitudCentroDeporte : src.CentroDeporte.Latitud))
                .ForMember(dest => dest.Longitud, opt => opt.MapFrom(src =>
                    src.OtroCentroDeporte ? src.LongitudCentroDeporte : src.CentroDeporte.Longitud))
                .ForMember(dest => dest.FechaEvento, opt => opt.MapFrom(src => DateTime.SpecifyKind(src.FechaEvento, DateTimeKind.Utc)));

            CreateMap<UsuarioEvento, GetEnrollmentRequestsByEvento.RequestDto>()
                    .ForMember(dest => dest.NombreUsuario, opt => opt.MapFrom(src => string.Join(" ", src.Usuario.Nombre, src.Usuario.ApellidoPaterno)))
                    .ForMember(dest => dest.Foto, opt => opt.MapFrom(src => src.Usuario.Foto));

            // Commands
            CreateMap<CreateEvento.Command, Evento>();                 

            CreateMap<UpdateEvento.Command, Evento>();
        }
    }
}
