// HelpTrack.Application/DTOs/CategoriaDTO.cs
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HelpTrack.Application.Resources;

namespace HelpTrack.Application.DTOs
{
    public class CategoriaDTO
    {
        public int IdCategoria { get; set; }

        [Display(Name = "Category Name")]
        public string Nombre { get; set; } = null!;

        [Display(Name = "Description")]
        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "SLA Required")]
        [Display(Name = "SLA")]
        public short IdSla { get; set; }

        [Display(Name = "Max Response Time")]
        public int TiempoMaxRespuestaHoras { get; set; }

        [Display(Name = "Max Resolution Time")]
        public int TiempoMaxResolucionHoras { get; set; }

        [Display(Name = "Tags")]
        public List<int> EtiquetasSeleccionadas { get; set; } = new List<int>();

        [Display(Name = "Specialties")]
        public List<int> EspecialidadesSeleccionadas { get; set; } = new List<int>();

        // Relaciones
        public ICollection<EspecialidadDTO> Especialidades { get; set; } = new List<EspecialidadDTO>();

        public ICollection<EtiquetaDTO> Etiquetas { get; set; } = new List<EtiquetaDTO>();
    }
}