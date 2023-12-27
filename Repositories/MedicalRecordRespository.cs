using Dapper;
using DataModels;
using DataModels.Config;
using Microsoft.EntityFrameworkCore;
using System.Data;
using static System.Net.Mime.MediaTypeNames;

namespace Repositories
{
    public class MedicalRecordRespository
    {
        private AppDbContext dbContext;
        private readonly DapperContext dapperContext;

        public MedicalRecordRespository(AppDbContext dbContext, DapperContext dapperContext)
        {
            this.dbContext = dbContext;
            this.dapperContext = dapperContext;
        }

        public async Task<MedicalRecord?> GetMedicalRecordByPhoneCustomer(string PhoneNumber)
        {
            var target = await dbContext.Customers.Where(c => c.PhoneNumber == PhoneNumber)
            .FirstOrDefaultAsync();
            var final = await dbContext.MedicalRecords.Where(mr => mr.CustomerId == target.Id)
                .FirstOrDefaultAsync();
            return final;
        }
        
        public async Task<(List<MedicalRecord>, int)> GetByIdDentist(string IdDentist, string text)
        {
			List<MedicalRecord> list = new List<MedicalRecord>();
			var param = new DynamicParameters();
			int count = 0;
			string procedureName = "TimKiemHSBA";
			param.Add("dentistId", IdDentist);
			param.Add("phoneNumber", text);
			param.Add("soLuongTimThay", dbType: DbType.Int32, direction: ParameterDirection.Output);
			SqlMapper.AddTypeHandler(new DapperSqlDateOnlyTypeHandler());
			using (var connection = dapperContext.CreateConnection())
			{
				try
				{
					var records = await connection
						.QueryAsync<MedicalRecord>(procedureName, param, commandType: CommandType.StoredProcedure);
					list = records.ToList();
					count = param.Get<int>("soLuongTimThay");
				}
				catch (Exception ex)
				{
					await Console.Out.WriteLineAsync("---------=====================----------------");
					await Console.Out.WriteLineAsync(ex.Message);
					await Console.Out.WriteLineAsync("---------=====================----------------");
					return (list, -1);
				}
			}
			return (list, count);
		}

        public async Task<MedicalRecord> GetById(int id, int sn)
        {
            var result = await dbContext.MedicalRecords.
                Where(mr => mr.Id == id && mr.SequenceNumber == sn).SingleOrDefaultAsync();
            return result;
        }

        public async Task<List<MedicalRecord>> GetByIdCustomer(string customerId)
        {
            List<MedicalRecord> result = new List<MedicalRecord>();
            var param = new DynamicParameters();
            string procedureName = "sp_CUSTOMER_SEE_RECORD";
            param.Add("idCustomer", customerId);
            SqlMapper.AddTypeHandler(new DapperSqlDateOnlyTypeHandler());
            using (var connection = dapperContext.CreateConnection())
            {
                try
                {
                    var records = await connection
                        .QueryAsync<MedicalRecord>(procedureName, param, commandType: CommandType.StoredProcedure);
                    result = records.ToList();
                }
                catch (Exception ex)
                {
                    await Console.Out.WriteLineAsync("---------=====================----------------");
                    await Console.Out.WriteLineAsync(ex.Message);
                    await Console.Out.WriteLineAsync("---------=====================----------------");
                    return result;
                }
            }
            return result;
        }
        public async Task<int> Add(MedicalRecord model)
        {
			var param = new DynamicParameters();
			string procedureName = "ThemHSBA";
			param.Add("id", model.Id, DbType.Int32);
			param.Add("lanKham", model.SequenceNumber, DbType.Int32);
			param.Add("ngayKham", model.ExaminationDate, DbType.Date);
			param.Add("dichVu", model.Service, DbType.String);
			param.Add("gia", model.ServicePrice, DbType.Decimal);
			param.Add("status", model.Status, DbType.String);
			param.Add("customerId", model.CustomerId, DbType.String);
			param.Add("idNhaSiTao", model.CreatedByDentistId, DbType.String);
			param.Add("idNhaSiKham", model.ExamDentistId, DbType.String);
			SqlMapper.AddTypeHandler(new DapperSqlDateOnlyTypeHandler());
			using (var connection = dapperContext.CreateConnection())
			{
				try
				{
					await connection
						.ExecuteAsync(procedureName, param, commandType: CommandType.StoredProcedure);
				}
				catch (Exception ex)
				{
					await Console.Out.WriteLineAsync("---------=====================----------------");
					await Console.Out.WriteLineAsync(ex.Message);
					await Console.Out.WriteLineAsync("---------=====================----------------");
					return 1;
				}
			}
			return 0;
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
            var param = new DynamicParameters();
            string procedureName = "sp_UPDATE_PATIENT_RECORD";
            param.Add("idHS", model.Id, DbType.Int32);
            param.Add("lankham", model.SequenceNumber, DbType.Int32);
            param.Add("date_time", model.ExaminationDate, DbType.Date);
            param.Add("dichvu", model.Service, DbType.String);
            param.Add("giadichvu", model.ServicePrice, DbType.Decimal);
            param.Add("tinhtrang", model.Status, DbType.String);
            param.Add("idCustomer", model.CustomerId, DbType.String);
            param.Add("idBsTao", model.CreatedByDentistId, DbType.String);
            param.Add("idBsKT", model.ExamDentistId, DbType.String);
            SqlMapper.AddTypeHandler(new DapperSqlDateOnlyTypeHandler());
            using (var connection = dapperContext.CreateConnection())
            {
                try
                {
                    await connection
                        .ExecuteAsync(procedureName, param, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    await Console.Out.WriteLineAsync("---------=====================----------------");
                    await Console.Out.WriteLineAsync(ex.Message);
                    await Console.Out.WriteLineAsync("---------=====================----------------");
                    return 1;
                }
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
