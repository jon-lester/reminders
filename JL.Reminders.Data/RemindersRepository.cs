using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Dapper;
using MySql.Data.MySqlClient;

using JL.Reminders.Core.Entities;
using JL.Reminders.Core.Repositories;
using JL.Reminders.Core.Model;

namespace JL.Reminders.Data
{
	public class RemindersRepository : IRemindersRepository
	{
		private readonly IConnectionStringFactory connectionStringFactory;

		public RemindersRepository(IConnectionStringFactory connectionStringFactory)
		{
			this.connectionStringFactory = connectionStringFactory;
		}

		public async Task<long> AddReminderAsync(string userId, ReminderEntity reminder)
		{
			using (MySqlConnection conn = new MySqlConnection(connectionStringFactory.GetConnectionString()))
			{
				await conn.ExecuteAsync(@"
					INSERT INTO reminders
					(
						UserID,
						Title,
						Description,
						ForDate,
						Created,
						Recurrence,
						Importance
					)
					VALUES
					(
						@userId,
						@title,
						@description,
						@fordate,
						UTC_TIMESTAMP(),
						@recurrence,
						@importance
					);",
					new
					{
						userId,
						title = reminder.Title,
						description = reminder.Description,
						fordate = reminder.ForDate,
						recurrence = reminder.Recurrence,
						importance = reminder.Importance
					});

				return await conn.QueryFirstAsync<long>("SELECT LAST_INSERT_ID();");
			}
		}

		public async Task<bool> DeleteReminderAsync(string userId, long reminderId)
		{
			using (MySqlConnection conn = new MySqlConnection(connectionStringFactory.GetConnectionString()))
			{
				var deleted = await conn.ExecuteAsync("DELETE FROM reminders WHERE UserID = @userId AND ID = @id;",
					new
					{
						userId,
						id = reminderId
					});
				return deleted > 0;
			}
		}

		public async Task<ReminderEntity> GetReminderByIdAsync(string userId, long reminderId)
		{
			using (MySqlConnection conn = new MySqlConnection(connectionStringFactory.GetConnectionString()))
			{
				return await conn.QueryFirstOrDefaultAsync<ReminderEntity>("SELECT ID, UserID, Title, Description, ForDate, Created, Recurrence, Importance, LastActioned FROM reminders WHERE UserID = @userId AND ID = @id;",
					new
					{
						userId,
						id = reminderId
					});
			}
		}

		public async Task<IEnumerable<ReminderEntity>> GetRemindersByUserIdAsync(string userId, ReminderStatus status = ReminderStatus.Active)
		{
			using (MySqlConnection conn = new MySqlConnection(connectionStringFactory.GetConnectionString()))
			{
				return await conn.QueryAsync<ReminderEntity>("SELECT ID, UserID, Title, Description, ForDate, Created, Recurrence, Importance, LastActioned FROM reminders WHERE UserID = @userId AND Status = @status;",
					new
					{
						userId,
						status = (int)status
					});
			}
		}

		public async Task<bool> SetReminderStatusAsync(string userId, long reminderId, ReminderStatus status)
		{
			using (MySqlConnection conn = new MySqlConnection(connectionStringFactory.GetConnectionString()))
			{
				var updated = await conn.ExecuteAsync(
					@"UPDATE reminders SET Status = @status WHERE ID = @id AND UserID = @userId;",
					new
					{
						id = reminderId,
						userId,
						status = (int)status,
					});

				return updated > 0;
			}
		}

		public async Task<bool> UpdateReminderAsync(string userId, ReminderEntity reminder)
		{
			using (MySqlConnection conn = new MySqlConnection(connectionStringFactory.GetConnectionString()))
			{
				var updated = await conn.ExecuteAsync(@"
					UPDATE reminders SET
						Title = @title,
						Description = @description,
						ForDate = @fordate,
						Recurrence = @recurrence,
						Importance = @importance
					WHERE ID = @id AND UserID = @userId;",
					new
					{
						title = reminder.Title,
						description = reminder.Description,
						fordate = reminder.ForDate,
						recurrence = reminder.Recurrence,
						importance = reminder.Importance,
						id = reminder.Id,
						userId
					});

				return updated > 0;
			}
		}

		public async Task<bool> UpdateReminderLastActionedAsync(string userId, long reminderId, DateTime lastActioned)
		{
			using (MySqlConnection conn = new MySqlConnection(connectionStringFactory.GetConnectionString()))
			{
				var updated = await conn.ExecuteAsync(@"
					UPDATE reminders SET
						LastActioned = @lastActioned
					WHERE ID = @id AND UserID = @userId;",
					new
					{
						lastActioned,
						id = reminderId,
						userId
					});

				return updated > 0;
			}
		}
	}
}
