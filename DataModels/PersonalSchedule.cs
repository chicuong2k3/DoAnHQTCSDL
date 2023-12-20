

using System.ComponentModel.DataAnnotations.Schema;

namespace DataModels
{
	public class PersonalSchedule
	{
        public DateTime StartTime { get; set; }
		public DateTime EndTime { get; set; }
		public Dentist Dentist { get; set; }
		[ForeignKey(nameof(Dentist))]
		public string DentistId { get; set; }
	}
}
