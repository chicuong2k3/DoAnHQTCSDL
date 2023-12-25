using DataModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication.Models
{
	public class EditMedicalRecordModel
	{
		public int Id { get; set; }
		public int SequenceNumber { get; set; }
		[Display(Name = "Ngày khám")]
        [DataType(DataType.Date)]
		public DateTime ExaminationDate { get; set; }
        [Display(Name = "Dịch vụ")]
        public string Service { get; set; }
		public decimal ServicePrice { get; set; } = 1200;
        [Display(Name = "Bệnh nhân")]
        public string CustomerId { get; set; }
        [Display(Name = "Nha sĩ tạo")]
        public string? CreatedByDentistId { get; set; }
        [Display(Name = "Nha sĩ khám")]
        public string? ExamDentistId { get; set; }
	}
}
