using System;

using JL.Reminders.Core.Model;

namespace JL.Reminders.Core.Services
{
	public interface IRemindersUtilityService
	{
		DateTime CalculateNextDueDate(Recurrence recurrence, DateTime forDate, DateTime? lastActioned = null);
		int CalculateDaysToGo(DateTime nextDueDate);
		string FormatReminderSubtitle(Recurrence recurrence, DateTime forDate);
	}
}
