using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using HelpTrack.Application.Resources;

namespace HelpTrack.Application.DTOs
{
    public class UsuarioDTO
    {
        public int IdUsuario { get; set; }

        [Required(ErrorMessage = "Email Required")]
        [EmailAddress(ErrorMessage = "Email Invalid")]
        [StringLength(100, ErrorMessage = "Email Length")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Name Required")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Name Length")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$", ErrorMessage = "Name Regex")]
        [Display(Name = "Full Name")]
        public string Nombre { get; set; } = string.Empty;

        [Display(Name = "Active")]
        public bool Activo { get; set; } = true;

        [Display(Name = "Creation Date")]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        [Display(Name = "Last Login")]
        public DateTime? UltimoInicioSesion { get; set; }

        public string? Password { get; set; }
    }
}
