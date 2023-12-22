using DataModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication.Models
{
	public class EditMedicalRecordModel
	{
		public int Id { get; set; }
		public int SequenceNumber { get; set; }
		public DateOnly ExaminationDate { get; set; }
		public string Service { get; set; }
		public decimal ServicePrice { get; set; } = 1200;
		public string CustomerId { get; set; }
		public string? CreatedByDentistId { get; set; }
		public string? ExamDentistId { get; set; }
	}
}
