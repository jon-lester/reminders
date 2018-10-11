using System;
using System.Threading.Tasks;

using Dapper;
using MySql.Data.MySqlClient;

using JL.Reminders.Core.Entities;
using JL.Reminders.Core.Repositories;

namespace JL.Reminders.Data
{
	public class UserPreferencesRepository : IUserPreferencesRepository
	{
		private readonly IConnectionStringFactory connectionStringFactory;

		public UserPreferencesRepository(IConnectionStringFactory connectionStringFactory)
		{
			this.connectionStringFactory = connectionStringFactory;
		}

		public async Task<PreferencesEntity> GetUserPreferencesAsync(string userId)
		{
			if (String.IsNullOrWhiteSpace(userId))
			{
				return null;
			}

			using (MySqlConnection conn = new MySqlConnection(connectionStringFactory.GetConnectionString()))
			{
				// returns null if no record found
				var preferences = await conn.QueryFirstOrDefaultAsync<PreferencesEntity>(@"
					SELECT ID, UserID, SoonDays, ImminentDays
						FROM preferences
						WHERE UserID = @userId;",
				new { userId });

				return preferences;
			}
		}

		public async Task<bool> SetUserPreferencesAsync(PreferencesEntity preferences)
		{
			if (preferences == null)
			{
				throw new ArgumentNullException($"{nameof(preferences)}");
			}

			using (MySqlConnection conn = new MySqlConnection(this.connectionStringFactory.GetConnectionString()))
			{
				var changed = await conn.ExecuteAsync(@"
					INSERT INTO preferences (UserID, SoonDays, ImminentDays)
						VALUES (@UserID, @SoonDays, @ImminentDays)
					ON DUPLICATE KEY UPDATE
						SoonDays=VALUES(SoonDays),
						ImminentDays=VALUES(ImminentDays);",
					new
					{
						preferences.UserID,
						preferences.SoonDays,
						preferences.ImminentDays
					});

				return changed > 0;
			}
		}
	}
}
