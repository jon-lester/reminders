using System;

using JL.Reminders.Core.Model;
using JL.Reminders.Core.Services;

namespace JL.Reminders.Services
{
	/// <summary>
	/// Methods related to the calculated properties of a Reminder object,
	/// intended to be resource-light so that it can run inside an iterator
	/// without causing load.
	/// </summary>
	public class RemindersUtilityService : IRemindersUtilityService
	{
		private readonly IDateTimeService dateTimeService;

		public RemindersUtilityService(IDateTimeService dateTimeService)
		{
			this.dateTimeService = dateTimeService;
		}

		public DateTime CalculateNextDueDate(Recurrence recurrence, DateTime forDate, DateTime? lastActioned = null)
		{
			if (recurrence == Recurrence.OneOff || lastActioned == null)
			{
				return forDate.Date;
			}

			Func<DateTime, DateTime> increment;

			switch (recurrence)
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
					throw new NotImplementedException($"Recurrence type not implemented: {recurrence.ToString()}");
			}

			DateTime nextDue = forDate.Date;

			while (nextDue <= lastActioned.Value)
			{
				nextDue = increment(nextDue);
			}

			return nextDue;
		}

		public int CalculateDaysToGo(DateTime nextDueDate)
		{
			var currentDate = dateTimeService.GetCurrentDate();
			return (int) Math.Round((nextDueDate.Date - currentDate).TotalDays);
		}

		public string FormatReminderSubtitle(Recurrence recurrence, DateTime forDate)
		{
			switch (recurrence)
			{
				case Recurrence.Annual:
					return $"Annual on {GetOrdinal(forDate.Day)} {forDate:MMM}.";
				case Recurrence.Monthly:
					return $"{GetOrdinal(forDate.Day)} of each month.";
				case Recurrence.OneOff:
					return $"{GetOrdinal(forDate.Day)} {forDate:MMMM, yyyy}";
				case Recurrence.Quarterly:
					return $"Quarterly from {GetOrdinal(forDate.Day)} {forDate:MMM}.";
				case Recurrence.SixMonthly:
					return $"{GetOrdinal(forDate.Day)} of {forDate:MMM} and {forDate.AddMonths(6):MMM}.";
				default:
					return String.Empty;
			}
		}

		private string GetOrdinal(int number)
		{
			return $"{number}{GetOrdinalIndicator(number)}";
		}

		private string GetOrdinalIndicator(int number)
		{
			if (number == 1 || number == 21 || number == 31)
			{
				return "st";
			}

			if (number == 2 || number == 22 || number == 32)
			{
				return "nd";
			}

			if (number == 3 || number == 23)
			{
				return "rd";
			}

			return "th";
		}
	}
}
