namespace DataModels
{
    public class Medicine
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int InventoryQuantity { get; set; }
        public string? Unit { get; set; }
        public string? Prescription { get; set; }
        public ICollection<Medicine_MedicalRecord> Medicine_MedicalRecords { get; set; }
    }
}
