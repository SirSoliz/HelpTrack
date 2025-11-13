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
        [ValidateEnumeratedItems(typeof(TicketDTO))]
        public int IdTicket { get; set; }

        [Display(Name = "Titulo")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        [ValidateEnumeratedItems(typeof(TicketDTO))]
        public string Titulo { get; set; } = null!;

        [Display(Name = "Descripcion")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        public string Descripcion { get; set; } = null!;

        public int IdPrioridad { get; set; }

        public int IdEstadoActual { get; set; }

        public int IdUsuarioCreacion { get; set; } 

        public EstadoTicketDTO? EstadoActual { get; set; }

        public virtual ICollection<ImagenesTicket> ImagenesTicket { get; set; } = new List<ImagenesTicket>();
        [ValidateNever]
        public virtual List<CategoriaDTO> IdCategoria { get; set; } = null!;
        [ValidateNever]
        public virtual TecnicoDTO IdTecnico { get; set; } = null!;

    }
}