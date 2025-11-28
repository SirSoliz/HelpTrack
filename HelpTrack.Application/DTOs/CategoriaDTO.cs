// HelpTrack.Application/DTOs/CategoriaDTO.cs
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HelpTrack.Application.DTOs
{
    public class CategoriaDTO
    {
        public int IdCategoria { get; set; }

        [Display(Name = "Nombre")]
        public string Nombre { get; set; } = null!;

        [Display(Name = "Descripción")]
        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "El campo SLA es obligatorio")]
        [Display(Name = "SLA")]
        public short IdSla { get; set; }

        [Display(Name = "Tiempo máximo de respuesta (horas)")]
        public int TiempoMaxRespuestaHoras { get; set; }

        [Display(Name = "Tiempo máximo de resolución (horas)")]
        public int TiempoMaxResolucionHoras { get; set; }

        [Display(Name = "Etiquetas")]
        public List<int> EtiquetasSeleccionadas { get; set; } = new List<int>();

        [Display(Name = "Especialidades")]
        public List<int> EspecialidadesSeleccionadas { get; set; } = new List<int>();

        // Relaciones
        public ICollection<EspecialidadDTO> Especialidades { get; set; } = new List<EspecialidadDTO>();

        public ICollection<EtiquetaDTO> Etiquetas { get; set; } = new List<EtiquetaDTO>();
    }
}