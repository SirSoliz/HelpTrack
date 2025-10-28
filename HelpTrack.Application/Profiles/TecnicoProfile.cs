using AutoMapper;
using HelpTrack.Application.DTOs;
using HelpTrack.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpTrack.Application.Profiles
{
    public class TecnicoProfile : Profile
    {
        public TecnicoProfile()
        {
            // Mapeo de TecnicoDTO a Tecnicos
            CreateMap<TecnicoDTO, Tecnicos>()
                .ForMember(dest => dest.IdTecnico, opt => opt.MapFrom(src => src.IdTecnico))
                .ForMember(dest => dest.Alias, opt => opt.MapFrom(src => src.Alias))
                .ForMember(dest => dest.Disponible, opt => opt.MapFrom(src => src.Disponible))
                .ForMember(dest => dest.NivelCarga, opt => opt.MapFrom(src => src.NivelCarga))
                .ForMember(dest => dest.IdTecnicoNavigation, opt => opt.MapFrom(src => src.Usuario));

            // Mapeo inverso de Tecnicos a TecnicoDTO
            CreateMap<Tecnicos, TecnicoDTO>()
                .ForMember(dest => dest.IdTecnico, opt => opt.MapFrom(src => src.IdTecnico))
                .ForMember(dest => dest.Alias, opt => opt.MapFrom(src => src.Alias))
                .ForMember(dest => dest.Disponible, opt => opt.MapFrom(src => src.Disponible))
                .ForMember(dest => dest.NivelCarga, opt => opt.MapFrom(src => src.NivelCarga))
                .ForMember(dest => dest.Usuario, opt => opt.MapFrom(src => src.IdTecnicoNavigation));

            // Mapeo entre UsuarioDTO y Usuarios
            CreateMap<UsuarioDTO, Usuarios>().ReverseMap();
        }
    }
}
