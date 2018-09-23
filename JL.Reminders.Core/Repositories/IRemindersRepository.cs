using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using JL.Reminders.Core.Entities;

namespace JL.Reminders.Core.Repositories
{
	public interface IRemindersRepository
	{
		Task<IEnumerable<ReminderEntity>> GetRemindersByUserIdAsync(string userId);
		Task<ReminderEntity> GetReminderByIdAsync(string userId, long reminderId);
		Task<long> AddReminderAsync(string userId, ReminderEntity reminder);
		Task<bool> DeleteReminderAsync(string userId, long reminderId);
		Task<bool> UpdateReminderAsync(string userId, ReminderEntity reminder);
		Task<bool> UpdateReminderLastActionedAsync(string userId, long reminderId, DateTime lastActioned);
	}
}