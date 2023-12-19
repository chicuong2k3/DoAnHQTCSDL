

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models
{
	public class CreateAppointmentModel
	{
		[Display(Name = "Thời gian hẹn")]
		public DateTime StartTime { get; set; }
		[DataType(DataType.Duration)]
        [Display(Name = "Khoảng thời gian muốn khám")]
        public int Duration { get; set; }
		[Display(Name = "Chọn nha sĩ khám (phút)")]
		public string DentistId { get; set;}
		public string CustomerId { get; set;}
	}
}
