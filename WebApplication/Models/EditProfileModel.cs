using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models
{
    public class EditProfileModel
    {
        [BindNever]
        [ValidateNever]
        public string Id { get; set; }
        [Display(Name = "Full Name")]
        public string? FullName { get; set; }
        public string? Address { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        public DateTime? DayOfBirth { get; set; }
    }
}
