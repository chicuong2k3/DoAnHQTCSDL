

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models
{
	public class CreateAppointmentModel
	{
		[Display(Name = "Start Time")]
		public DateTime StartTime { get; set; }
		[DataType(DataType.Duration)]
        public int Duration { get; set; }
		public string DentistId { get; set;}
		public string CustomerId { get; set;}
	}
}
