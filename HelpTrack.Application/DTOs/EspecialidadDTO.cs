// HelpTrack.Application/DTOs/EspecialidadDTO.cs
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HelpTrack.Application.DTOs
{
    public class EspecialidadDTO
    {
        public short IdEspecialidad { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(100, ErrorMessage = "El nombre no puede tener más de 100 caracteres")]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "La descripción no puede tener más de 500 caracteres")]
        public string? Descripcion { get; set; }

        // Si necesitas incluir información de las categorías relacionadas
        public ICollection<CategoriaDTO>? Categorias { get; set; }
    }
}