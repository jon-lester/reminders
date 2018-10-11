using System;
using System.Threading.Tasks;

using JL.Reminders.Core.Entities;

namespace JL.Reminders.Core.Repositories
{
	public interface IUserPreferencesRepository
	{
		Task<PreferencesEntity> GetUserPreferencesAsync(string userId);
		Task<bool> SetUserPreferencesAsync(PreferencesEntity preferences);
	}
}
