using System.ComponentModel.DataAnnotations;

namespace HelpTrack.ViewModels
{
    public class RolSeleccionableViewModel
    {
        public int IdRol { get; set; }

        [Display(Name = "Nombre del Rol")]
        public string Nombre { get; set; } = string.Empty;

        [Display(Name = "Descripción")]
        public string? Descripcion { get; set; }

        public bool Seleccionado { get; set; }
    }
}
