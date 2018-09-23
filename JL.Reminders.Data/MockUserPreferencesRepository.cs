using System;
using System.Threading.Tasks;

using JL.Reminders.Core.Model;
using JL.Reminders.Core.Repositories;

namespace JL.Reminders.Data
{
	public class MockUserPreferencesRepository : IUserPreferencesRepository
	{
		public Task<UserPreferences> GetUserPreferencesAsync(String userId)
		{
			return Task.FromResult(new UserPreferences
			{
				UrgencyConfiguration = new UrgencyConfiguration
				{
					ImminentDays = 7,
					SoonDays = 30
				}
			});
		}

		public Task SetUserPreferencesAsync(String userId, UserPreferences preferences)
		{
			return Task.CompletedTask;
		}
	}
}
