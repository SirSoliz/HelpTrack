using System.ComponentModel.DataAnnotations;

namespace HelpTrack.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email Required")]
        [EmailAddress(ErrorMessage = "Email Invalid")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password Required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
