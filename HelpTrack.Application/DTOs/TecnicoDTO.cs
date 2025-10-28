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

        [Display(Name = "Alias")]
        [Required(ErrorMessage = "{0} es un dato requerido")]

        public string? Alias { get; set; }

        public bool Disponible { get; set; }

        public byte NivelCarga { get; set; }

        // Propiedad de navegación al usuario relacionado
        public virtual UsuarioDTO? Usuario { get; set; }
    }
}