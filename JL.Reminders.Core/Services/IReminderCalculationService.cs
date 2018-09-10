using System;

using JL.Reminders.Core.Model;

namespace JL.Reminders.Core.Services
{
	public interface IReminderCalculationService
	{
		int CalculateDaysToGo(Reminder reminder);
		DateTime CalculateNextDueDate(Reminder reminder);
	}
}
