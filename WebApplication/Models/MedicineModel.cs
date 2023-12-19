using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace WebApplication.Models
{
	public class MedicineModel
	{
		public int MedicineId { get; set; }
		[Display(Name = "Tên thuốc")]
		public string Name { get; set; }
        [Display(Name = "Chỉ định")]
        public string? Prescription { get; set; }
        [Display(Name = "Số lượng")]
        public int Quantity { get; set; }
        [Display(Name = "Đơn vị tính")]
        public string? Unit { get; set; }
	}
}
