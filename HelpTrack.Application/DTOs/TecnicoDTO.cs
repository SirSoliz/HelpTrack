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
    public record TecnicoDTO
    {
        [Display(Name = "Id Tecnico")]
        [ValidateNever]
        public int IdTecnico { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        public string Nombre { get; set; } = null!;
        // public virtual List<Tecnicos> Tecnico { get; set; } = new List<Tecnicos>();
    }
}