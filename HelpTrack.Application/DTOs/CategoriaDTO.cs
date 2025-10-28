// HelpTrack.Application/DTOs/CategoriaDTO.cs
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HelpTrack.Application.DTOs
{
    public class CategoriaDTO
    {
        public short IdCategoria { get; set; }

        [Display(Name = "Nombre")]
        public string Nombre { get; set; } = null!;

        [Display(Name = "Descripción")]
        public string? Descripcion { get; set; }

        [Display(Name = "SLA")]
        public short IdSla { get; set; }

        [Display(Name = "Tiempo máximo de respuesta (horas)")]
        public int TiempoMaxRespuestaHoras { get; set; }

        [Display(Name = "Tiempo máximo de resolución (horas)")]
        public int TiempoMaxResolucionHoras { get; set; }

        // Relaciones
        [Display(Name = "Especialidades")]
        public ICollection<EspecialidadDTO> Especialidades { get; set; } = new List<EspecialidadDTO>();

        [Display(Name = "Etiquetas")]
        public ICollection<EtiquetaDTO> Etiquetas { get; set; } = new List<EtiquetaDTO>();
    }
}