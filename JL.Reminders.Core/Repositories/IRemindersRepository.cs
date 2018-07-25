using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using JL.Reminders.Core.Model;

namespace JL.Reminders.Core.Repositories
{
	public interface IRemindersRepository
	{
		Task<IEnumerable<Reminder>> GetRemindersByUserIdAsync(long userId);
		Task<Reminder> GetReminderByIdAsync(long userId, long reminderId);
		Task<long> AddReminderAsync(long userId, Reminder reminder);
		Task<bool> DeleteReminderAsync(long userId, long reminderId);
		Task<bool> UpdateReminderAsync(long userId, Reminder reminder);
	}
}