using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Dapper;
using MySql.Data.MySqlClient;

using JL.Reminders.Core.Model;
using JL.Reminders.Core.Repositories;

namespace JL.Reminders.Data
{
	public class RemindersRepository : IRemindersRepository
	{
		private readonly IConnectionStringFactory connectionStringFactory;

		public RemindersRepository(IConnectionStringFactory connectionStringFactory)
		{
			this.connectionStringFactory = connectionStringFactory;
		}

		public async Task<long> AddReminderAsync(string userId, Reminder reminder)
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

		public async Task<Reminder> GetReminderByIdAsync(string userId, long reminderId)
		{
			using (MySqlConnection conn = new MySqlConnection(connectionStringFactory.GetConnectionString()))
			{
				return await conn.QueryFirstOrDefaultAsync<Reminder>("SELECT ID, UserID, Title, Description, ForDate, Created, Recurrence, Importance, LastActioned FROM reminders WHERE UserID = @userId AND ID = @id;",
					new
					{
						userId,
						id = reminderId
					});
			}
		}

		public async Task<IEnumerable<Reminder>> GetRemindersByUserIdAsync(string userId)
		{
			using (MySqlConnection conn = new MySqlConnection(connectionStringFactory.GetConnectionString()))
			{
				return await conn.QueryAsync<Reminder>("SELECT ID, UserID, Title, Description, ForDate, Created, Recurrence, Importance, LastActioned FROM reminders WHERE UserID = @userId;",
					new
					{
						userId
					});
			}
		}

		public async Task<bool> UpdateReminderAsync(string userId, Reminder reminder)
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
						id = reminder.ID,
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
