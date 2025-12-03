using System;
using System.ComponentModel.DataAnnotations;

namespace HelpTrack.Application.DTOs
{
    public record TicketHistoryDTO
    {
        public int IdTicket { get; set; }

        [Display(Name = "Título")]
        public string Titulo { get; set; } = null!;

        [Display(Name = "Fecha de Creación")]
        public DateTime FechaCreacion { get; set; }

        [Display(Name = "Fecha de Asignación")]
        public DateTime? FechaAsignacion { get; set; }

        [Display(Name = "Técnico Asignado")]
        public string? NombreTecnico { get; set; }

        [Display(Name = "Estado")]
        public string Estado { get; set; } = null!;
    }
}
