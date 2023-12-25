using Dapper;
using DataModels;
using DataModels.Config;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Repositories
{
	public class MedicineInventoryRepository
	{
		private AppDbContext dbContext;
		private DapperContext dapperContext;
		public MedicineInventoryRepository(AppDbContext dbContext,
			DapperContext dapperContext)
		{
			this.dbContext = dbContext;
			this.dapperContext = dapperContext;
		}

		public async Task<List<MedicineInventory>> GetAllMedicineInventory()
		{
			var data = await dbContext.MedicineInventories.ToListAsync();
			return data;
		}

		public async Task InsertMedicineInvetory(MedicineInventory model)
		{
			await dbContext.MedicineInventories.AddAsync(model);
			await dbContext.SaveChangesAsync();
		}

		public async Task<int> Delete(int id)
		{
			var item = await dbContext.MedicineInventories.FindAsync(id);
			if (item != null)
			{
				dbContext.MedicineInventories.Remove(item);
				await dbContext.SaveChangesAsync();
				return 1;
			}
			return 0;
		}

		public async Task<int> Update(MedicineInventory model)
		{
			//var item = await dbContext.MedicineInventories.FindAsync(model.Id);
			//if (item != null)
			//{
			//	dbContext.Entry(item).CurrentValues.SetValues(model);
			//	await dbContext.SaveChangesAsync();
			//}

			int check = 0; //thanh cong
						   //use procedure 
						   //UPDATE_QUANTITY_DEADLOCK2
			var param = new DynamicParameters();
			string procedureName = "UPDATE_QUANTITY_DEADLOCK2";
			//@ID INT, @QUANTITY int
			param.Add("ID", model.Id);
			param.Add("QUANTITY", model.InventoryQuantity);
			using (var connection = dapperContext.CreateConnection())
			{
				try
				{
					await connection.ExecuteAsync(procedureName, param, commandType: CommandType.StoredProcedure);
				}
				catch (Exception ex)
				{
					if (ex.Message.Contains("deadlock"))
					{
						await Console.Out.WriteLineAsync("---------=====================----------------");
						await Console.Out.WriteLineAsync(ex.Message);
						await Console.Out.WriteLineAsync("---------=====================----------------");
						check = 1; //deadlock
					}
					else
					{
						check = 2; //khong tim thay thuoc nay
					}
				}
			}
			return check;
        }

        public async Task<MedicineInventory?> GetByKey(int id)
		{
			return await dbContext.MedicineInventories.FindAsync(id);
		}

		public async Task<int> DeleteAllExpirededicine()
		{
			string procName = "DELETE_EXPIRED_MEDICINE_DEADLOCK2";
			var param = new DynamicParameters();
			var check = 0; //thanh cong
			using(var connection = dapperContext.CreateConnection())
			{
				try
				{
					await connection.ExecuteAsync(procName, param, 
						commandType : CommandType.StoredProcedure);
				}
				catch(Exception ex)
				{
                    await Console.Out.WriteLineAsync("---------------------================-----------------");
                    await Console.Out.WriteLineAsync(ex.Message);
                    await Console.Out.WriteLineAsync("---------------------================-----------------");
					if (ex.Message.Contains("deadlock"))
					{
						check = 1; //deadlock
					}
					else
					{
						//loi khac
						check = 2;
					}
				}
			}
			return check;
		}
	}
}