using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HelpTrack.Application.DTOs
{
    public record AsignacionTicketDTO
    {
        public int IdAsignacion { get; set; }

        [Required(ErrorMessage = "El ID del ticket es requerido")]
        public int IdTicket { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un técnico")]
        [Display(Name = "Técnico")]
        public int IdTecnico { get; set; }

        [Required(ErrorMessage = "El método de asignación es requerido")]
        [Display(Name = "Método")]
        public string Metodo { get; set; } = "Manual";

        [Display(Name = "Prioridad")]
        [Range(1, 5, ErrorMessage = "La prioridad debe estar entre 1 y 5")]
        public int Prioridad { get; set; } = 3;

        [Display(Name = "Justificación")]
        [Required(ErrorMessage = "La justificación es obligatoria")]
        [StringLength(500, ErrorMessage = "La justificación no puede exceder los 500 caracteres")]
        public string Justificacion { get; set; } = string.Empty;

        public DateTime? FechaAsignacion { get; set; }

        // Propiedad para el SelectList de técnicos disponibles
        [ValidateNever]
        public SelectList? TecnicosDisponibles { get; set; }

        // Navigation properties
        [ValidateNever]
        public virtual TicketDTO? Ticket { get; set; }

        [ValidateNever]
        public virtual TecnicoDTO? Tecnico { get; set; }
    }
}