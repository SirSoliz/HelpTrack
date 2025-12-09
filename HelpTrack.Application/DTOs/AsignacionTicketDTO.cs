using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using HelpTrack.Resources;

namespace HelpTrack.Application.DTOs
{
    public record AsignacionTicketDTO
    {
        public int IdAsignacion { get; set; }

        [Required(ErrorMessage = "Ticket Id Required")]
        public int IdTicket { get; set; }

        [Required(ErrorMessage = "Technician Required")]
        [Display(Name = "Technician")]
        public int IdTecnico { get; set; }

        [Required(ErrorMessage = "Method Required")]
        [Display(Name = "Method")]
        public string Metodo { get; set; } = "Manual";

        [Display(Name = "Priority")]
        [Range(1, 5, ErrorMessage = "Priority Range")]
        public int Prioridad { get; set; } = 3;

        [Display(Name = "Justification")]
        [Required(ErrorMessage = "Justification Required")]
        [StringLength(500, ErrorMessage = "JustificationLength")]
        public string Justificacion { get; set; } = string.Empty;

        public DateTime? FechaAsignacion { get; set; }

        // Propiedad para el SelectList de técnicos disponibles
        [ValidateNever]
        public SelectList? TecnicosDisponibles { get; set; }

        // Propiedad para el SelectList de prioridades disponibles
        [ValidateNever]
        public SelectList? PrioridadesDisponibles { get; set; }

        // Navigation properties
        [ValidateNever]
        public virtual TicketDTO? Ticket { get; set; }

        [ValidateNever]
        public virtual TecnicoDTO? Tecnico { get; set; }
    }
}