using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using JL.Reminders.Core.Model;
using JL.Reminders.Core.Repositories;

namespace JL.Reminders.Data
{
	public class MockRemindersRepository : IRemindersRepository
	{
		public Task<long> AddReminderAsync(long userId, Reminder reminder)
		{
			return Task.FromResult(1L);
		}

		public Task<bool> DeleteReminderAsync(long userId, long reminderId)
		{
			return Task.FromResult(true);
		}

		public Task<Reminder> GetReminderByIdAsync(long userId, long reminderId)
		{
			return Task.FromResult(new Reminder());
		}

		public Task<IEnumerable<Reminder>> GetRemindersByUserIdAsync(long userId)
		{
			return Task.FromResult(new List<Reminder>
			{
				new Reminder
				{
					Recurrence = Recurrence.Annual,
					ForDate = DateTime.UtcNow,
					LastActioned = null,
					Description = "Annual reminder 1",
					DaysToGo = 0,
					Importance = Importance.Important,
					Title = "Annual 1",
					SubTitle = "Annual reminder",
					Created = DateTime.UtcNow,
					ID = 1
				},
				new Reminder
				{
					Recurrence = Recurrence.Annual,
					ForDate = DateTime.UtcNow,
					LastActioned = null,
					Description = "Annual reminder 2",
					DaysToGo = 0,
					Importance = Importance.Important,
					Title = "Annual 2",
					SubTitle = "Annual reminder",
					Created = DateTime.UtcNow,
					ID = 2
				},
				new Reminder
				{
					Recurrence = Recurrence.Annual,
					ForDate = DateTime.UtcNow,
					LastActioned = null,
					Description = "Annual reminder 3",
					DaysToGo = 0,
					Importance = Importance.Important,
					Title = "Annual 3",
					SubTitle = "Annual reminder",
					Created = DateTime.UtcNow,
					ID = 3
				}
			} as IEnumerable<Reminder>);
		}

		public Task<bool> UpdateReminderAsync(long userId, Reminder reminder)
		{
			return Task.FromResult(true);
		}

		public Task<bool> UpdateReminderLastActionedAsync(long userId, long reminderId, DateTime lastActioned)
		{
			return Task.FromResult(true);
		}
	}
}
