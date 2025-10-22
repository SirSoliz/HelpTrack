using HelpTrack.Infraestructure.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpTrack.Application.DTOs
{
    public record CategoriaDTO
    {
        public int IdCategoria { get; set; }

        public string Nombre { get; set; } = null!;

        public virtual List<TicketDTO> idTicket { get; set; } = null!;
    }
}
