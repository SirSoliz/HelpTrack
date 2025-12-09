using System.ComponentModel.DataAnnotations;

namespace HelpTrack.Application.DTOs
{
    public class RolDTO
    {
        public int IdRol { get; set; }

        [Required(ErrorMessage = "Role Name Required")]
        [StringLength(50, ErrorMessage = "Role Name Length")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$", ErrorMessage = "Role Name Regex")]
        [Display(Name = "Role Name")]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(200, ErrorMessage = "Description Length")]
        [Display(Name = "Description")]
        public string? Descripcion { get; set; }
    }
}
