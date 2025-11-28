// CategoriaProfile.cs
using AutoMapper;
using HelpTrack.Application.DTOs;
using HelpTrack.Infraestructure.Models;

public class CategoriaProfile : Profile
{
    // CategoriaProfile.cs
    public CategoriaProfile()
    {
        // Mapeo de Categorias a CategoriaDTO
        CreateMap<Categorias, CategoriaDTO>()
            .ForMember(dest => dest.TiempoMaxRespuestaHoras,
                       opt => opt.MapFrom(src => src.IdSlaNavigation != null ? src.IdSlaNavigation.TiempoRespuestaMax : 0))
            .ForMember(dest => dest.TiempoMaxResolucionHoras,
                       opt => opt.MapFrom(src => src.IdSlaNavigation != null ? src.IdSlaNavigation.TiempoResolucionMax : 0))
            .ForMember(dest => dest.Especialidades,
                       opt => opt.MapFrom(src => src.IdEspecialidad))
            .ForMember(dest => dest.Etiquetas,
                       opt => opt.MapFrom(src => src.IdEtiqueta));

        // Mapeo de CategoriaDTO a Categorias
        CreateMap<CategoriaDTO, Categorias>()
            .ForMember(dest => dest.IdSlaNavigation, opt => opt.Ignore()) // Ignoramos el mapeo de IdSlaNavigation
            .ForMember(dest => dest.IdEspecialidad, opt => opt.Ignore())  // Ignoramos el mapeo de IdEspecialidad
            .ForMember(dest => dest.IdEtiqueta, opt => opt.Ignore());     // Ignoramos el mapeo de IdEtiqueta

        // Mapeos adicionales
        CreateMap<Especialidades, EspecialidadDTO>().ReverseMap();
        CreateMap<Etiquetas, EtiquetaDTO>().ReverseMap();
    }
}