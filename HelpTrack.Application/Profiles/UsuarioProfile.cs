using AutoMapper;
using HelpTrack.Application.DTOs;
using HelpTrack.Infraestructure.Models;

namespace HelpTrack.Application.Profiles
{
    public class UsuarioProfile : Profile
    {
        public UsuarioProfile()
        {
            CreateMap<UsuarioDTO, Usuarios>()
                .ForMember(dest => dest.IdUsuario, opt => opt.Ignore()) // Se ignora porque es generado por la base de datos
                .ForMember(dest => dest.Contrasena, opt => opt.Ignore()) // La contraseña se maneja por separado
                .ReverseMap();
        }
    }
}
