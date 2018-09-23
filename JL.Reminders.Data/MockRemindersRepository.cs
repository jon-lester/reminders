using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using JL.Reminders.Core.Entities;
using JL.Reminders.Core.Model;
using JL.Reminders.Core.Repositories;

namespace JL.Reminders.Data
{
	public class MockRemindersRepository : IRemindersRepository
	{
		public Task<long> AddReminderAsync(string userId, ReminderEntity reminder)
		{
			return Task.FromResult(1L);
		}

		public Task<bool> DeleteReminderAsync(string userId, long reminderId)
		{
			return Task.FromResult(true);
		}

		public Task<ReminderEntity> GetReminderByIdAsync(string userId, long reminderId)
		{
			return Task.FromResult(new ReminderEntity());
		}

		public Task<IEnumerable<ReminderEntity>> GetRemindersByUserIdAsync(string userId)
		{
			return Task.FromResult(new List<ReminderEntity>
			{
				new ReminderEntity
				{
					Recurrence = Recurrence.Annual,
					ForDate = DateTime.UtcNow,
					LastActioned = null,
					Description = "Annual reminder 1",
					Importance = Importance.Important,
					Title = "Annual 1",
					Created = DateTime.UtcNow,
					Id = 1
				},
				new ReminderEntity
				{
					Recurrence = Recurrence.Annual,
					ForDate = DateTime.UtcNow,
					LastActioned = null,
					Description = "Annual reminder 2",
					Importance = Importance.Important,
					Title = "Annual 2",
					Created = DateTime.UtcNow,
					Id = 2
				},
				new ReminderEntity
				{
					Recurrence = Recurrence.Annual,
					ForDate = DateTime.UtcNow,
					LastActioned = null,
					Description = "Annual reminder 3",
					Importance = Importance.Important,
					Title = "Annual 3",
					Created = DateTime.UtcNow,
					Id = 3
				}
			} as IEnumerable<ReminderEntity>);
		}

		public Task<bool> UpdateReminderAsync(string userId, ReminderEntity reminder)
		{
			return Task.FromResult(true);
		}

		public Task<bool> UpdateReminderLastActionedAsync(string userId, long reminderId, DateTime lastActioned)
		{
			return Task.FromResult(true);
		}
	}
}
