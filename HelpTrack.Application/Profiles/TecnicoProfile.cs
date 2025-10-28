using AutoMapper;
using HelpTrack.Application.DTOs;
using HelpTrack.Infraestructure.Models;

public class TecnicoProfile : Profile
{
    public TecnicoProfile()
    {
        CreateMap<Tecnicos, TecnicoDTO>()
            .ForMember(dest => dest.Usuario,
                      opt => opt.MapFrom(src => src.IdTecnicoNavigation))
            .ForMember(dest => dest.Especialidades,
                      opt => opt.MapFrom(src => src.IdEspecialidad))
            .ReverseMap();
    }
}