using AutoMapper;
using DataModels;
using WebApplication.Models;

namespace WebApplication.Congif
{
    public class ConfigAutoMapper : Profile
    {
        public ConfigAutoMapper()
        {
            CreateMap<Medicine, CreateMedicineModel>().ReverseMap();

        }
    }
}
