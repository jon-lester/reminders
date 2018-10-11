using System;

using AutoMapper;

using JL.Reminders.Core.Entities;
using JL.Reminders.Core.Model;
using JL.Reminders.Core.Services.Interfaces;

namespace JL.Reminders.Core.Services
{
	/// <summary>
	/// Given a Reminder and the user's preferences, calculate all fields
	/// which are calculable from its static data. This service should not
	/// interact with the database, ie. it's intended to be resource-light
	/// so that it can run inside an iterator without causing load.
	/// </summary>
	public class ReminderHydrationService : IReminderHydrationService
	{
		private readonly IDateTimeService dateTimeService;

		public ReminderHydrationService(IDateTimeService dateTimeService)
		{
			this.dateTimeService = dateTimeService;
		}

		public Reminder HydrateReminder(ReminderEntity reminder)
		{
			Reminder hydratedReminder = Mapper.Map<ReminderEntity, Reminder>(reminder, opts => opts.ConfigureMap(MemberList.Source));

			hydratedReminder.NextDueDate = CalculateNextDueDate(hydratedReminder);
			hydratedReminder.DaysToGo = CalculateDaysToGo(hydratedReminder);
			hydratedReminder.SubTitle = GetReminderSubtitle(hydratedReminder);

			return hydratedReminder;
		}

		private DateTime CalculateNextDueDate(Reminder reminder)
		{
			if (reminder.Recurrence == Recurrence.OneOff || reminder.LastActioned == null)
			{
				return reminder.ForDate.Date;
			}

			Func<DateTime, DateTime> increment;

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

			DateTime nextDue = reminder.ForDate.Date;

			while (nextDue <= reminder.LastActioned.Value)
			{
				nextDue = increment(nextDue);
			}

			return nextDue;
		}

		private int CalculateDaysToGo(Reminder reminder)
		{
			var nextDueDate = CalculateNextDueDate(reminder);
			var currentDate = dateTimeService.GetCurrentDate();
			return (int) Math.Round((nextDueDate - currentDate).TotalDays);
		}

		private string GetReminderSubtitle(Reminder r)
		{
			switch (r.Recurrence)
			{
				case Recurrence.Annual:
					return $"Annual on {GetOrdinal(r.ForDate.Day)} {r.ForDate:MMM}.";
				case Recurrence.Monthly:
					return $"{GetOrdinal(r.ForDate.Day)} of each month.";
				case Recurrence.OneOff:
					return $"{GetOrdinal(r.ForDate.Day)} {r.ForDate:MMMM, yyyy}";
				case Recurrence.Quarterly:
					return $"Quarterly from {GetOrdinal(r.ForDate.Day)} {r.ForDate:MMM}.";
				case Recurrence.SixMonthly:
					return $"{GetOrdinal(r.ForDate.Day)} of {r.ForDate:MMM} and {r.ForDate.AddMonths(6):MMM}.";
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
