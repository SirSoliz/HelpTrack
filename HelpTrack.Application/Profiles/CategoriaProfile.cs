// CategoriaProfile.cs
using AutoMapper;
using HelpTrack.Application.DTOs;
using HelpTrack.Infraestructure.Models;

namespace HelpTrack.Application.Profiles
{
    public class CategoriaProfile : Profile
    {
        // CategoriaProfile.cs
        public CategoriaProfile()
        {
            // Mapeo de Categorias a CategoriaDTO
            CreateMap<Categorias, CategoriaDTO>()
                .ForMember(dest => dest.TiempoMaxRespuestaHoras,
                         opt => opt.MapFrom(src => src.IdSlaNavigation != null ?
                             src.IdSlaNavigation.TiempoRespuestaMax : 0))
                .ForMember(dest => dest.TiempoMaxResolucionHoras,
                         opt => opt.MapFrom(src => src.IdSlaNavigation != null ?
                             src.IdSlaNavigation.TiempoResolucionMax : 0))
                .ForMember(dest => dest.EtiquetasSeleccionadas,
                         opt => opt.MapFrom(src => src.IdEtiqueta.Select(e => e.IdEtiqueta).ToList()))
                .ForMember(dest => dest.EspecialidadesSeleccionadas,
                         opt => opt.MapFrom(src => src.IdEspecialidad.Select(e => e.IdEspecialidad).ToList()));

            // Mapeo de CategoriaDTO a Categorias
            CreateMap<CategoriaDTO, Categorias>()
                .ForMember(dest => dest.IdSlaNavigation, opt => opt.Ignore())
                .ForMember(dest => dest.IdEtiqueta, opt => opt.Ignore())
                .ForMember(dest => dest.IdEspecialidad, opt => opt.Ignore());
        }

    }
}