using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models
{
	public class MedicineInventoryModel
	{
		public int Id { get; set; }
		[Required]
		public int MedicineId { get; set; }
		[Required]
		public DateOnly ExpiryDate { get; set; }
		[Required]
		public int InventoryQuantity { get; set; }
		[Required]
		public string? Unit { get; set; }
	}
}
