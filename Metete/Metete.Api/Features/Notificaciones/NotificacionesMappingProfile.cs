using AutoMapper;
using Metete.Api.Features.Notificaciones.Queries;
using Metete.Api.Features.Notificacions.Queries;
using Metete.Api.Models;

namespace Metete.Api.Features.TipoDeportes
{
    public class NotificacionesProfile : Profile
    {
        public NotificacionesProfile()
        {
            // Queries
            CreateMap<Notificacion, GetPendingPushNotificaciones.NotificacionDto>()
                .ForMember(dest => dest.FcmToken, opt => opt.MapFrom(src => src.Usuario.FcmToken));

            CreateMap<Notificacion, GetMyNotificaciones.NotificacionDto>()
                 .ForMember(dest => dest.Titulo, opt => opt.MapFrom(src => src.TipoNotificacion.Titulo))
                 .ForMember(dest => dest.FechaEnvio, opt => opt.MapFrom(src => 
                    src.FechaEnvio.HasValue ? DateTime.SpecifyKind(src.FechaEnvio.Value, DateTimeKind.Utc) : (DateTime?)null));
        }
    }
}
