using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using JL.Reminders.Core.Entities;
using JL.Reminders.Core.Model;

namespace JL.Reminders.Core.Repositories
{
	public interface IRemindersRepository
	{
		Task<IEnumerable<ReminderEntity>> GetRemindersByUserIdAsync(string userId, ReminderStatus status);
		Task<ReminderEntity> GetReminderByIdAsync(string userId, long reminderId);
		Task<long> AddReminderAsync(string userId, ReminderEntity reminder);
		Task<bool> DeleteReminderAsync(string userId, long reminderId);
		Task<bool> UpdateReminderAsync(string userId, ReminderEntity reminder);
		Task<bool> UpdateReminderLastActionedAsync(string userId, long reminderId, DateTime lastActioned);
		Task<bool> SetReminderStatusAsync(string userId, long reminderId, ReminderStatus status);
	}
}