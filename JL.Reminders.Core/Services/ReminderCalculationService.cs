using System;

using JL.Reminders.Core.Model;

namespace JL.Reminders.Core.Services
{
	public class ReminderCalculationService : IReminderCalculationService
	{
		private readonly IDateTimeService dateTimeService;

		public ReminderCalculationService(IDateTimeService dateTimeService)
		{
			this.dateTimeService = dateTimeService;
		}

		public DateTime CalculateNextDueDate(Reminder reminder)
		{
			if (reminder.Recurrence == Recurrence.OneOff || reminder.LastActioned == null)
			{
				return reminder.ForDate;
			}

			Func<DateTime, DateTime> increment = null;

			switch (reminder.Recurrence)
			{
				case Recurrence.Annual:
					increment = dt => dt.AddYears(1);
					break;
				case Recurrence.Monthly:
					increment = dt => dt.AddMonths(1);
					break;
				case Recurrence.Quarterly:
					increment = dt => dt.AddMonths(3);
					break;
				case Recurrence.SixMonthly:
					increment = dt => dt.AddMonths(6);
					break;
				default:
					throw new NotImplementedException($"Recurrence type not implemented: {reminder.Recurrence.ToString()}");
			}

			DateTime nextDue = reminder.ForDate;

			while (nextDue <= reminder.LastActioned.Value)
			{
				nextDue = increment(nextDue);
			}

			return nextDue;
		}

		public int CalculateDaysToGo(Reminder reminder)
		{
			return (int) Math.Floor((CalculateNextDueDate(reminder) - dateTimeService.GetCurrentDateTime()).TotalDays);
		}
	}
}
