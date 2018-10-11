using System.Threading.Tasks;
using JL.Reminders.Core.Model;

namespace JL.Reminders.Core.Services.Interfaces
{
	public interface IUserPreferencesService
	{
		Task<UserPreferences> GetUserPreferencesAsync(string userId);
		Task<bool> SetUserPreferencesAsync(string userId, UserPreferences userPreferences);
	}
}
