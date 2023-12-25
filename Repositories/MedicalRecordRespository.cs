using DataModels;
using Microsoft.EntityFrameworkCore;

namespace Repositories
{
    public class MedicalRecordRespository
    {
        private AppDbContext dbContext;
        public MedicalRecordRespository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<MedicalRecord?> GetMedicalRecordByPhoneCustomer(string PhoneNumber)
        {
            var target = await dbContext.Customers.Where(c => c.PhoneNumber == PhoneNumber)
            .FirstOrDefaultAsync();
            var final = await dbContext.MedicalRecords.Where(mr => mr.CustomerId == target.Id)
                .FirstOrDefaultAsync();
            return final;
        }
        
        public async Task<List<MedicalRecord>> GetByIdDentist(string IdDentist)
        {
            var result = new List<MedicalRecord>();
            result = await dbContext.MedicalRecords.
                Where(mr => mr.ExamDentistId == IdDentist).ToListAsync();
            return result;
        }

        public async Task<List<MedicalRecord>> GetByIdCustomer(string customerId)
        {
            var result = new List<MedicalRecord>();
            if(!dbContext.MedicalRecords.Any())
            {
                return result;
            }
            else return await dbContext.MedicalRecords.Where(mr => mr.CustomerId == customerId)
                .OrderByDescending(c => c.ExaminationDate)
                .ToListAsync();
        }
        public async Task Add(MedicalRecord model)
        {
            await dbContext.MedicalRecords.AddAsync(model);
            await dbContext.SaveChangesAsync();
        }

        public async Task<int> Delete(int id, int sequence)
        {
            var target = await dbContext.MedicalRecords.Where(mr => (mr.Id == id && mr.SequenceNumber == sequence))
                .FirstOrDefaultAsync();
            if(target != null)
            {
                dbContext.MedicalRecords.Remove(target);
                await dbContext.SaveChangesAsync();
                return 1;
            }
            return 0;
        }

        public async Task<int> Edit(MedicalRecord model)
        {
            var target = await dbContext.MedicalRecords.Where(mr =>
            (mr.Id == model.Id && mr.SequenceNumber == model.SequenceNumber)).FirstOrDefaultAsync();
            if(target != null )
            {
                dbContext.MedicalRecords.Entry(target).CurrentValues.SetValues(model);
                await dbContext.SaveChangesAsync();
                return 1;
            }
            return 0;
        }

        public async Task<MedicalRecord?> GetLatestMedicalRecordByCustomerId(string customerId)
        {
            if(await dbContext.MedicalRecords.CountAsync() == 0)
            {
                return null;
            }
            var target = await dbContext.MedicalRecords.Where(mr => mr.CustomerId == customerId)
                .OrderByDescending(mr => mr.SequenceNumber).FirstOrDefaultAsync();
            return target;
        }

        public async Task<MedicalRecord?> GetMaxId()
        {
            var target = await dbContext.MedicalRecords.OrderBy(m => m.Id).FirstOrDefaultAsync();
            return target;
        }
    }
}
