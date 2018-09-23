using System;
using System.Threading.Tasks;

using JL.Reminders.Core.Model;

namespace JL.Reminders.Core.Repositories
{
	public interface IUserPreferencesRepository
	{
		Task<UserPreferences> GetUserPreferencesAsync(string userId);
		Task SetUserPreferencesAsync(string userId, UserPreferences preferences);
	}
}
