using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DataModels
{
    public class ExaminationSchedule
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime ExaminationTime { get; set; }
        public bool IsAvailable { get; set; }
        public Dentist Dentist { get; set; }
        [ForeignKey(nameof(Dentist))]
        public string DentistId { get; set; }
    }
}
