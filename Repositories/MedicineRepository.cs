using AutoMapper;
using DataModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class MedicineRepository
    {
        private AppDbContext dbContext;
        private IMapper mapper;
        public MedicineRepository(AppDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }
        public async Task<List<Medicine>> GetAllMedicine()
        {
            return await dbContext.Medicines.ToListAsync();
        }

        public async Task<Medicine?> GetMedicineById(int id)
        {
            return await dbContext.Medicines.Where(m => m.Id == id)
                .Include(m => m.Medicine_MedicalRecords).FirstOrDefaultAsync();
        }

        public async Task UpdateMedicine(Medicine medicine)
        {
            var check = await dbContext.Medicines.FindAsync(medicine.Id);
            if(check != null)
            {
                dbContext.Medicines.Entry(check).CurrentValues.SetValues(medicine);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task RemoveMedicine(int id)
        {
            var item = await dbContext.Medicines.FindAsync(id);
            if(item != null)
            {
                dbContext.Medicines.Remove(item);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task AddMedicine(Medicine model)
        {
            await dbContext.Medicines.AddAsync(model);
            await dbContext.SaveChangesAsync();
        }
    }
}
