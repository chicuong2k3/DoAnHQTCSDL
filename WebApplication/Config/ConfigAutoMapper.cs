using AutoMapper;
using DataModels;
using WebApplication.Models;

namespace WebApplication.Congfig
{
    public class ConfigAutoMapper : Profile
    {
        public ConfigAutoMapper()
        {
            CreateMap<Medicine, CreateMedicineModel>().ReverseMap();
            CreateMap<MedicineInventory, MedicineInventoryModel>().ReverseMap();
            CreateMap<MedicalRecord, EditMedicalRecordModel>().ReverseMap();
        }
    }
}
