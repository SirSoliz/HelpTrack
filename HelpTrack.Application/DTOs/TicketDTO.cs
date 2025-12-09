using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using HelpTrack.Application.DTOs;
using HelpTrack.Infraestructure.Models;
using Microsoft.AspNetCore.Http;
using HelpTrack.Resources;

namespace HelpTrack.Application.DTOs
{
    public record TicketDTO
    {
        [Display(Name = "TicketID")]
        [ValidateNever]
        public int IdTicket { get; set; }

        [Required(ErrorMessage = "Title Required")]
        [StringLength(150)]
        [Display(Name = "Title")]
        public string Titulo { get; set; } = null!;

        [Display(Name = "Description")]
        [Required(ErrorMessage = "Description Required")]
        public string Descripcion { get; set; } = null!;

        [Display(Name = "Priority")]
        [Required(ErrorMessage = "Priority Required")]
        public int IdPrioridad { get; set; }

        [Display(Name = "Category")] 
        [Required(ErrorMessage = "Category Required")]
        public int IdCategoria { get; set; }

        [Display(Name = "Creator User")]
        public int IdUsuarioCreacion { get; set; }

        [Display(Name = "Created By")]
        public string? NombreUsuarioCreacion { get; set; }

        [Display(Name = "Status")]
        public int IdEstadoActual { get; set; }

        [ValidateNever]
        public EstadoTicketDTO? EstadoActual { get; set; }

        [ValidateNever]
        public virtual ICollection<ImagenesTicket> ImagenesTicket { get; set; } = new List<ImagenesTicket>();
        
        [ValidateNever]
        public virtual TecnicoDTO? Tecnico { get; set; }

        [Display(Name = "New Images")]
        [ValidateNever]
        public List<IFormFile>? NuevasImagenes { get; set; }
    }
}