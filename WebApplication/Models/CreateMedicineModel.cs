namespace WebApplication.Models
{
    public class CreateMedicineModel
    {
        public int MedicineId { get; set; }
        public string Name { get; set; }
        public string? Prescription { get; set; }
        public int Quantity { get; set; }
        public string? Unit { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
