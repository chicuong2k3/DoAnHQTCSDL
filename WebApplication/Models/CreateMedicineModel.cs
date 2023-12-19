using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models
{
    public class CreateMedicineModel
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
        [Display(Name = "Ngày hết hạn")]
        [DataType(DataType.Date)]
        public DateTime ExpiryDate { get; set; }
    }
}
