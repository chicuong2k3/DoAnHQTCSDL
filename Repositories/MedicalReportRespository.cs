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
            try
            {
            var target = await dbContext.Customers.Where(c => c.PhoneNumber == PhoneNumber)
                .Select(c => dbContext.MedicalRecords.Where(mr => mr.CustomerId == c.Id).FirstOrDefault())
                .OrderByDescending(c => c.SequenceNumber)
                .FirstOrDefaultAsync();
                return target;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
                return null;
            }
        }
        
        public async Task<List<MedicalRecord>> GetByIdDentist(string IdDentist)
        {
            return await dbContext.MedicalRecords.Where(mr => mr.ExamDentistId == IdDentist).ToListAsync();
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
                dbContext.Entry(target).CurrentValues.SetValues(model);
                await dbContext.SaveChangesAsync();
                return 1;
            }
            return 0;
        }

        public async Task<MedicalRecord?> GetLatestMedicalRecordByCustomerId(string customerId)
        {
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
