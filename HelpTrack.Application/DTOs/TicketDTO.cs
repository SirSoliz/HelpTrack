using HelpTrack.Application.DTOs;
using HelpTrack.Infraestructure.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        [Display(Name = "Descripcion")]
        [Required(ErrorMessage = "La descripción es obligatoria.")]
        public string Descripcion { get; set; } = null!;
        [Required]
        public int IdPrioridad { get; set; }

        public int IdUsuarioCreacion { get; set; }
        [ValidateNever]
        public EstadoTicketDTO? EstadoActual { get; set; }
        [ValidateEnumeratedItems]
        public virtual ICollection<ImagenesTicket> ImagenesTicket { get; set; } = new List<ImagenesTicket>();
        [ValidateNever]
        public virtual List<CategoriaDTO> IdCategoria { get; set; } = null!;
        [ValidateNever]
        public virtual TecnicoDTO IdTecnico { get; set; } = null!;

    }
}