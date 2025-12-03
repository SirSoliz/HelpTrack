using System.ComponentModel.DataAnnotations;

namespace HelpTrack.Application.DTOs
{
    public class ActualizarEstadoTicketDTO
    {
        [Required]
        public int IdTicket { get; set; }

        [Required(ErrorMessage = "El nuevo estado es requerido")]
        public int IdNuevoEstado { get; set; }

        [Required(ErrorMessage = "El comentario es requerido")]
        [StringLength(500, ErrorMessage = "El comentario no puede exceder los 500 caracteres")]
        public string Comentario { get; set; } = string.Empty;

        public int IdUsuario { get; set; }
    }
}