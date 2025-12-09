using System.ComponentModel.DataAnnotations;

namespace HelpTrack.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Name Required")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Email Required")]
        [EmailAddress(ErrorMessage = "Email Invalid")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password Required")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "Password Length", MinimumLength = 6)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "ConfirmPassword")]
        [Compare("Password", ErrorMessage = "Password Mismatch")]
        public string ConfirmPassword { get; set; }
    }
}
