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
            CreateMap<TicketDTO, Tickets>().ReverseMap();

            CreateMap<TicketDTO, Tickets>()
            .ForMember(dest => dest.IdTicket, orig => orig.MapFrom(o => o.IdTicket))
            .ForMember(dest => dest.IdPrioridad, orig => orig.MapFrom(o => o.IdPrioridad))
            .ForMember(dest => dest.IdUsuarioCreacion, orig => orig.MapFrom(o => o.IdUsuarioCreacion))
            .ForMember(dest => dest.Titulo, orig => orig.MapFrom(o => o.Titulo))
            .ForMember(dest => dest.Descripcion, orig => orig.MapFrom(o => o.Descripcion))
            .ForMember(dest => dest.ImagenesTicket, orig => orig.MapFrom(o => o.ImagenesTicket))
            .ForMember(dest => dest.IdCategoria, orig => orig.MapFrom(o => o.IdCategoria));

        }
    }
}
