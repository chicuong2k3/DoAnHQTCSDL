using Dapper;
using DataModels;
using DataModels.Config;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Repositories
{
	public class AppointmentScheduleRepository
	{
		private readonly AppDbContext dbContext;
		private readonly DapperContext dapperContext;

		public AppointmentScheduleRepository(AppDbContext dbContext, DapperContext dapperContext)
		{
			this.dbContext = dbContext;
			this.dapperContext = dapperContext;
		}

        public async Task<List<AppointmentSchedule>> GetAppointmentsOfDentist(string dentistId)
		{
			List<AppointmentSchedule> list = new List<AppointmentSchedule>();
			var param = new DynamicParameters();
			string procedureName = "Xem_Lich_Hen";
			param.Add("IdBS", dentistId);
			using (var connection = dapperContext.CreateConnection())
			{
				try
				{
					list = (await connection
						.QueryAsync<AppointmentSchedule>(procedureName, param, commandType: CommandType.StoredProcedure)).ToList();
				}
				catch (Exception ex)
				{
					await Console.Out.WriteLineAsync("---------=====================----------------");
					await Console.Out.WriteLineAsync(ex.Message);
					await Console.Out.WriteLineAsync("---------=====================----------------");
					return null;
				}
			}
			return list;
		}

        public async Task<List<AppointmentSchedule>> GetAppointmentsOfCustomer(string customerId)
        {
            return await dbContext.AppointmentSchedules
                .Where(x => !string.IsNullOrEmpty(x.CustomerId) 
				&& x.CustomerId == customerId
				&& x.StartTime >= DateTime.Now)
				.ToListAsync();
        }

        public async Task AddAppoinment(AppointmentSchedule appointment)
        {
			var param = new DynamicParameters();
			string procedureName = "Them_Lich_Hen";
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

		public async Task<List<AppointmentSchedule>> GetAllSchedulesBelongToADentist(string dentistId)
		{
			return await dbContext.AppointmentSchedules
				.Where(x => x.DentistId == dentistId).ToListAsync();
		}

		public async Task<AppointmentSchedule> FindAsync(string dentistId, DateTime startTime)
		{
			return await dbContext.AppointmentSchedules
				.Where(x => x.DentistId == dentistId && x.StartTime == startTime).SingleOrDefaultAsync();
		}

		public async Task UpdateAsync(AppointmentSchedule schedule, DateTime sTime, DateTime eTime, string dentistId = null)
		{
			var newSchedule = new AppointmentSchedule()
			{
				StartTime = sTime,
				EndTime = eTime,
				CustomerId = schedule.CustomerId
			};
			if (dentistId == null)
			{
				newSchedule.DentistId = schedule.DentistId;
			}
			else
			{
				newSchedule.DentistId = dentistId;
			}
            dbContext.AppointmentSchedules.Remove(schedule);
            await dbContext.SaveChangesAsync();
            dbContext.AppointmentSchedules.Add(newSchedule);
			await dbContext.SaveChangesAsync();
		}
	}
}
