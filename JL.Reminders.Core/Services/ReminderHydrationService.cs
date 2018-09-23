using System;
using AutoMapper;

using JL.Reminders.Core.Entities;
using JL.Reminders.Core.Model;

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

		public Reminder HydrateReminder(ReminderEntity reminder, UrgencyConfiguration urgencyDefaults)
		{
			Reminder hydratedReminder = Mapper.Map<ReminderEntity, Reminder>(reminder, opts => opts.ConfigureMap(MemberList.Source));

			hydratedReminder.NextDueDate = CalculateNextDueDate(hydratedReminder);
			hydratedReminder.DaysToGo = CalculateDaysToGo(hydratedReminder);
			hydratedReminder.SubTitle = GetReminderSubtitle(hydratedReminder);
			hydratedReminder.Urgency = CalculateUrgency(hydratedReminder, urgencyDefaults);

			return hydratedReminder;
		}

		private DateTime CalculateNextDueDate(Reminder reminder)
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

		private int CalculateDaysToGo(Reminder reminder)
		{
			return (int) Math.Floor((CalculateNextDueDate(reminder) - dateTimeService.GetCurrentDateTime()).TotalDays);
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

		public Urgency CalculateUrgency(Reminder reminder, UrgencyConfiguration defaults)
		{
			// get the defaults to use
			int soonDays = defaults.SoonDays;
			int imminentDays = defaults.ImminentDays;

			// if the reminder has a full configuration of its own,
			// use those values instead
			if (reminder.SoonDaysPreference != null && reminder.ImminentDaysPreference != null)
			{
				soonDays = reminder.SoonDaysPreference.Value;
				imminentDays = reminder.ImminentDaysPreference.Value;
			}

			Urgency urgency = default(Urgency);

			if (reminder.DaysToGo < 0)
			{
				urgency = Urgency.Overdue;
			}
			else if (reminder.DaysToGo == 0)
			{
				urgency = Urgency.Now;
			}
			else if (reminder.DaysToGo <= imminentDays)
			{
				urgency = Urgency.Imminent;
			}
			else if (reminder.DaysToGo <= soonDays)
			{
				urgency = Urgency.Soon;
			}
			else
			{
				urgency = Urgency.Normal;
			}

			return urgency;
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
