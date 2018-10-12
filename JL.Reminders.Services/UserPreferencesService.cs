using System;
using System.Threading.Tasks;

using JL.Reminders.Core.Entities;
using JL.Reminders.Core.Model;
using JL.Reminders.Core.Repositories;
using JL.Reminders.Core.Services;

namespace JL.Reminders.Services
{
	public class UserPreferencesService : IUserPreferencesService
	{
		private readonly IUserPreferencesRepository userPreferencesRepository;

		public UserPreferencesService(IUserPreferencesRepository userPreferencesRepository)
		{
			this.userPreferencesRepository = userPreferencesRepository;
		}

		public async Task<UserPreferences> GetUserPreferencesAsync(string userId)
		{
			UserPreferences userPreferences = new UserPreferences();

			var prefs = await userPreferencesRepository.GetUserPreferencesAsync(userId);

			if (prefs == null)
			{
				userPreferences.UrgencyConfiguration = new UrgencyConfiguration
				{
					ImminentDays = 7,
					SoonDays = 14
				};
			}
			else
			{
				userPreferences.UrgencyConfiguration = new UrgencyConfiguration
				{
					ImminentDays = prefs.ImminentDays,
					SoonDays = prefs.SoonDays
				};
			}

			return userPreferences;
		}

		public async Task<bool> SetUserPreferencesAsync(string userId, UserPreferences userPreferences)
		{
			return await this.userPreferencesRepository.SetUserPreferencesAsync(new PreferencesEntity
			{
				UserID = userId,
				ImminentDays = userPreferences.UrgencyConfiguration.ImminentDays,
				SoonDays = userPreferences.UrgencyConfiguration.SoonDays
			});
		}
	}
}