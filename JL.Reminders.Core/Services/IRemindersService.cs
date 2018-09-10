using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using JL.Reminders.Core.Model;

namespace JL.Reminders.Core.Services
{
	public interface IRemindersService
	{
		Task<IEnumerable<Reminder>> GetRemindersByUserIdAsync(long userId);
		Task<ReminderOptions> GetReminderOptions();
		Task<Reminder> GetReminderByIdAsync(long userId, long reminderId);
		Task<long> AddReminderAsync(long userId, Reminder reminder);
		Task<bool> DeleteReminderAsync(long userId, long reminderId);
		Task<bool> UpdateReminderAsync(long userId, Reminder reminder);
		Task<bool> ActionReminderAsync(long userId, ReminderAction action);
	}
}
