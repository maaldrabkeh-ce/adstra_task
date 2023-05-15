using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace adstra_task.ViewModels
{
    public class UserViewModel
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "Confirm Password")]
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and confirmation password not match.")]
        public string ConfirmPassword { get; set; }
        public string UserImage { get; set; }
        public IFormFile File { get; set; }
    }

}
