using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataModels
{
    public class AppointmentSchedule
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime AppointmentTime { get; set; }
        public Dentist Dentist { get; set; }
        [ForeignKey(nameof(Dentist))]   
        public string DentistId { get; set; }
        public Customer Customer { get; set; }
        [ForeignKey(nameof(Customer))]
        public string CustomerId { get; set; }
    }
}
