namespace WebApplication.Models
{
    public class CreateMedicineModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Prescription { get; set; }
    }
}