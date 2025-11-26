using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using HelpTrack.Application.DTOs;
using HelpTrack.Infraestructure.Models;
using Microsoft.AspNetCore.Http;

namespace HelpTrack.Application.DTOs
{
    public record TicketDTO
    {
        [Display(Name = "Id Ticket")]
        [ValidateNever]
        public int IdTicket { get; set; }

        [Required(ErrorMessage = "El título es obligatorio.")]
        [StringLength(150)]
        public string Titulo { get; set; } = null!;

        [Display(Name = "Descripción")]
        [Required(ErrorMessage = "La descripción es obligatoria.")]
        public string Descripcion { get; set; } = null!;

        [Display(Name = "Prioridad")]
        [Required(ErrorMessage = "La prioridad es obligatoria.")]
        public int IdPrioridad { get; set; }

        [Display(Name = "Categoría")]
        [Required(ErrorMessage = "La categoría es obligatoria.")]
        public int IdCategoria { get; set; }

        [Display(Name = "Usuario creador")]
        public int IdUsuarioCreacion { get; set; }

        [Display(Name = "Creado por")]
        public string? NombreUsuarioCreacion { get; set; }

        [Display(Name = "Estado")]
        public int IdEstadoActual { get; set; }

        [ValidateNever]
        public EstadoTicketDTO? EstadoActual { get; set; }

        [ValidateNever]
        public virtual ICollection<ImagenesTicket> ImagenesTicket { get; set; } = new List<ImagenesTicket>();
        
        [ValidateNever]
        public virtual TecnicoDTO? Tecnico { get; set; }

        [Display(Name = "Nuevas Imágenes")]
        [ValidateNever]
        public List<IFormFile>? NuevasImagenes { get; set; }
    }
}