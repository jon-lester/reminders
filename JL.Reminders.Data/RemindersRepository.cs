using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;
using JL.Reminders.Core.Model;
using JL.Reminders.Core.Repositories;
using MySql.Data.MySqlClient;

namespace JL.Reminders.Data
{
	public class RemindersRepository : IRemindersRepository
	{
		private readonly IConnectionStringFactory connectionStringFactory;

		public RemindersRepository(IConnectionStringFactory connectionStringFactory)
		{
			this.connectionStringFactory = connectionStringFactory;
		}

		public async Task<long> AddReminderAsync(long userId, Reminder reminder)
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
						Importance = reminder.Importance
					});

				return await conn.QueryFirstAsync<long>("SELECT LAST_INSERT_ID();");
			}
		}

		public async Task<bool> DeleteReminderAsync(long userId, long reminderId)
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

		public async Task<Reminder> GetReminderByIdAsync(long userId, long reminderId)
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

		public async Task<IEnumerable<Reminder>> GetRemindersByUserIdAsync(long userId)
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

		public async Task<bool> UpdateReminderAsync(long userId, Reminder reminder)
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
	}
}
