using AutoMapper;
using HelpTrack.Application.DTOs;
using HelpTrack.Infraestructure.Models;
using System.Linq;

namespace HelpTrack.Application.Profiles
{
    public class TicketHistoryProfile : Profile
    {
        public TicketHistoryProfile()
        {
            CreateMap<Tickets, TicketHistoryDTO>()
                .ForMember(dest => dest.IdTicket, opt => opt.MapFrom(src => src.IdTicket))
                .ForMember(dest => dest.Titulo, opt => opt.MapFrom(src => src.Titulo))
                .ForMember(dest => dest.FechaCreacion, opt => opt.MapFrom(src => src.FechaCreacion))
                .ForMember(dest => dest.FechaAsignacion, opt => opt.MapFrom(src => src.FechaAsignacion))
                .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => src.IdEstadoActualNavigation.Nombre))
                // Map Technician Name: Ticket -> AsignacionesTicket (collection) -> FirstOrDefault -> IdTecnicoNavigation -> Usuario -> Nombre
                // Note: AsignacionesTicket is a collection in the model, but logically a ticket has one active assignment usually.
                // We'll take the most recent one or the one matching the current technician if possible.
                // For simplicity, we assume the navigation property 'AsignacionesTicket' (singular in DB context but might be collection in EF)
                // Checking Tickets.cs: public virtual AsignacionesTicket? AsignacionesTicket { get; set; } 
                // Wait, in Tickets.cs it was: public virtual AsignacionesTicket? AsignacionesTicket { get; set; }
                // Let's re-verify Tickets.cs content from previous turn.
                // Step 53: public virtual AsignacionesTicket? AsignacionesTicket { get; set; }
                // It seems it's a 1:0..1 relation in the model file shown? 
                // Actually Step 53 says: public virtual AsignacionesTicket? AsignacionesTicket { get; set; }
                // But Step 54 (AsignacionesTicket.cs) has: public virtual Tickets IdTicketNavigation { get; set; } = null!;
                // If it is 1:1, then:
                .ForMember(dest => dest.NombreTecnico, opt => opt.MapFrom(src => src.AsignacionesTicket != null && src.AsignacionesTicket.IdTecnicoNavigation != null && src.AsignacionesTicket.IdTecnicoNavigation.IdTecnicoNavigation != null ? src.AsignacionesTicket.IdTecnicoNavigation.IdTecnicoNavigation.Nombre : "Sin Asignar"));
        }
    }
}
