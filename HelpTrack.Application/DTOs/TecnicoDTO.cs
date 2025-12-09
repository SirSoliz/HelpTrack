using HelpTrack.Application.DTOs;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using HelpTrack.Application.Resources;

namespace HelpTrack.Application.DTOs
{
    public record TecnicoDTO
    {
        [Display(Name = "Technician ID")]
        [ValidateNever]
        public int IdTecnico { get; set; }

        [Display(Name = "Alias")] 
        [Required(ErrorMessage = "Alias Required")]
        public string? Alias { get; set; }

        public bool Disponible { get; set; }

        [Display(Name = "Workload Level")]
        [Range(0, 10, ErrorMessage = "Workload Range")]
        public byte NivelCarga { get; set; } = 0;

      

        // Propiedad de navegación al usuario relacionado
        public virtual UsuarioDTO? Usuario { get; set; }

        // Lista de especialidades
        public virtual ICollection<EspecialidadDTO>? Especialidades { get; set; } = new List<EspecialidadDTO>();


        
    }
}