using DataModels;

namespace Repositories
{

    public class MyPrescriptionMedicine
	{
		public Medicine_MedicalRecord medicine_MedicalRecord { set; get; }
		public Medicine medicine { set; get; }
	}
	public class InformationPrescriptionModel
	{
		public Customer customer { set; get; }
		public MedicalRecord medicalRecord { set; get; }
		public List<MyPrescriptionMedicine>? listMedicine { set; get; }
	}
}
