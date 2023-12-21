using DataModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication.Models
{
    public class MedicalRecordModel
    {
		public DateOnly ExaminationDate { get; set; }
		public string Service { get; set; }
		public string PhoneNumber { get; set; }
		public string CreatedByDentistId { get; set; }
    }
}
