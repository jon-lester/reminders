using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using JL.Reminders.Core.Model;

namespace JL.Reminders.Core.Services
{
	public interface IRemindersService
	{
		Task<IEnumerable<Reminder>> GetRemindersByUserIdAsync(string userId);
		Task<ReminderOptions> GetReminderOptions();
		Task<Reminder> GetReminderByIdAsync(string userId, long reminderId);
		Task<long> AddReminderAsync(string userId, Reminder reminder);
		Task<bool> DeleteReminderAsync(string userId, long reminderId);
		Task<bool> UpdateReminderAsync(string userId, Reminder reminder);
		Task<bool> ActionReminderAsync(string userId, ReminderAction action);
	}
}
