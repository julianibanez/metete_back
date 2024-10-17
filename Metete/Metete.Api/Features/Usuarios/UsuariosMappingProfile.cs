using AutoMapper;
using Metete.Api.Features.Usuarios.Command;
using Metete.Api.Features.Usuarios.Queries;
using Metete.Api.Models;

namespace Metete.Api.Features.TipoDeportes
{
    public class UsuariosMappingProfile : Profile
    {
        public UsuariosMappingProfile()
        {
            // Queries
            CreateMap<Usuario, GetContextUsuario.UsuarioDto>()
                 .ForMember(dest => dest.NombreNacionalidad, opt => opt.MapFrom(src => src.Nacionalidad.Nombre))
                 .ForMember(dest => dest.NombreComuna, opt => opt.MapFrom(src => src.Comuna.Nombre));

            CreateMap<Usuario, GetUsuariosForChat.UsuarioDto>()
                 .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => string.Join(" ", src.Nombre, src.ApellidoPaterno)));

            // Commands
            CreateMap<UpdateContextUsuario.Command, Usuario>();
        }
    }
}
