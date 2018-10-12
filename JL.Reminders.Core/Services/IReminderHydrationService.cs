using JL.Reminders.Core.Entities;
using JL.Reminders.Core.Model;

namespace JL.Reminders.Core.Services
{
	public interface IReminderHydrationService
	{
		Reminder HydrateReminder(ReminderEntity reminder);
	}
}
