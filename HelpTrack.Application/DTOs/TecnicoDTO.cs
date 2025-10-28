using HelpTrack.Application.DTOs;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

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

        [Display(Name = "Nivel de Carga")]
        [Range(0, 10, ErrorMessage = "El nivel de carga debe estar entre 0 y 10")]
        public byte NivelCarga { get; set; }

        // Propiedad de navegación al usuario relacionado
        public virtual UsuarioDTO? Usuario { get; set; }

        // Lista de especialidades
        public virtual ICollection<EspecialidadDTO>? Especialidades { get; set; } = new List<EspecialidadDTO>();
    }
}