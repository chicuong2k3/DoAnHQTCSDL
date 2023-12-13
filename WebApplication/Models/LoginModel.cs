using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models
{
    public class LoginModel
    {
        [Display(Name = "User Name or Phone Number")]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
    }
}
