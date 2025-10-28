// CategoriaProfile.cs
using AutoMapper;
using HelpTrack.Application.DTOs;
using HelpTrack.Infraestructure.Models;

public class CategoriaProfile : Profile
{
    public CategoriaProfile()
    {
        // Mapas para elementos de colección (si no existen en otro perfil)
        CreateMap<Especialidades, EspecialidadDTO>();
        CreateMap<Etiquetas, EtiquetaDTO>();

        CreateMap<Categorias, CategoriaDTO>()
            .ForMember(dest => dest.TiempoMaxRespuestaHoras,
                       opt => opt.MapFrom(src => src.IdSlaNavigation != null ? src.IdSlaNavigation.TiempoRespuestaMax : 0))
            .ForMember(dest => dest.TiempoMaxResolucionHoras,
                       opt => opt.MapFrom(src => src.IdSlaNavigation != null ? src.IdSlaNavigation.TiempoResolucionMax : 0))
            .ForMember(dest => dest.Especialidades,
                       opt => opt.MapFrom(src => src.IdEspecialidad))
            .ForMember(dest => dest.Etiquetas,
                       opt => opt.MapFrom(src => src.IdEtiqueta))
            .ReverseMap();
    }
}