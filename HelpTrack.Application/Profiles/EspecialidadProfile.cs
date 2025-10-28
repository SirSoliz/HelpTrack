// HelpTrack.Application/Profiles/EspecialidadProfile.cs
using AutoMapper;
using HelpTrack.Application.DTOs;
using HelpTrack.Infraestructure.Models;

namespace HelpTrack.Application.Profiles
{
    public class EspecialidadProfile : Profile
    {
        public EspecialidadProfile()
        {
            CreateMap<Especialidades, EspecialidadDTO>()
                .ReverseMap();
        }
    }
}