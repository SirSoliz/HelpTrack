using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpTrack.Application.DTOs
{
    public record PrioridadDTO
    {
        [Display(Name = "Id Prioridad")]
        [ValidateNever]
        public int IdPrioridad { get; set; }
        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        public string Nombre { get; set; } = null!;
        [ValidateNever]
        public int ValorPrioridad { get; set; }

    }
}
