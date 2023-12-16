using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models
{
    public class EditProfileModel
    {
        [Display(Name = "Phone Number")]
        //[Remote(action: "IsAccountAvailable", controller: "Account")]
        public string PhoneNumber { get; set; }
        public string FullName { get; set; }
        public string? Address { get; set; }
        [DataType(DataType.Date)]
        public DateTime? DayOfBirth { get; set; }
    }
}
