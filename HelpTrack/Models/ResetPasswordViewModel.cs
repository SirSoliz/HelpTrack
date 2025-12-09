using System.ComponentModel.DataAnnotations;

namespace HelpTrack.Models
{
    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "Email Required")]
        [EmailAddress(ErrorMessage = "Email Invalid")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password Required")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "Password Length", MinimumLength = 6)]
        [Display(Name = "NewPassword")]
        public string NewPassword { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Display(Name = "ConfirmPassword")]
        [Compare("NewPassword", ErrorMessage = "Password Mismatch")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
