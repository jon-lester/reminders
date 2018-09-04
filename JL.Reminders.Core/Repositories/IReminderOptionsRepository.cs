using System.Threading.Tasks;

using JL.Reminders.Core.Model;

namespace JL.Reminders.Core.Repositories
{
	public interface IReminderOptionsRepository
	{
		Task<ReminderOptions> GetReminderOptions();
	}
}