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
    public class TicketProfile : Profile
    {
        public TicketProfile()
        {
            CreateMap<TicketDTO, Tickets>()
                .ForMember(dest => dest.IdTicket, opt => opt.MapFrom(src => src.IdTicket))
                .ForMember(dest => dest.IdPrioridad, opt => opt.MapFrom(src => src.IdPrioridad))
                .ForMember(dest => dest.Titulo, opt => opt.MapFrom(src => src.Titulo))
                .ForMember(dest => dest.Descripcion, opt => opt.MapFrom(src => src.Descripcion))
                .ForMember(dest => dest.IdUsuarioCreacion, opt => opt.MapFrom(src => src.IdUsuarioCreacion))
                .ForMember(dest => dest.ImagenesTicket, opt => opt.MapFrom(src => src.ImagenesTicket))
                .ForMember(dest => dest.IdCategoria, opt => opt.MapFrom(src => src.IdCategoria))
    .ForMember(dest => dest.IdCategoria, opt => opt.Ignore())
    .ForMember(dest => dest.IdEstadoActual, opt => opt.Ignore())
    .ForMember(dest => dest.IdSla, opt => opt.Ignore())
    .ForMember(dest => dest.IdEtiqueta, opt => opt.Ignore());

            CreateMap<Tickets, TicketDTO>().ReverseMap();
        }
    }
}
