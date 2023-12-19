using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models
{
    public class RegisterModel
    {
        [Display(Name = "Số điện thoại")]
        //[Remote(action: "IsAccountAvailable", controller: "Account")]
        public string PhoneNumber { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Xác nhận mật khẩu")]
        [Compare(nameof(Password), ErrorMessage = "Không khớp với mật khẩu")]
        public string ConfirmPassword { get; set; }
        [Display(Name = "Họ và tên")]
        public string FullName { get; set; }
        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Ngày sinh")]
        public DateTime DayOfBirth { get; set; }
	}
}
