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
    public class HistorialTicketProfile : Profile
    {
        public HistorialTicketProfile()
        {
            CreateMap<HistorialTicket, HistorialTicketDTO>()
                .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => src.IdEstadoNavigation.Nombre))
                .ForMember(dest => dest.UsuarioAccion, opt => opt.MapFrom(src => src.IdUsuarioAccionNavigation.Nombre));
        }
    }
}
