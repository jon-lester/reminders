using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using JL.Reminders.Core.Model;
using JL.Reminders.Core.Repositories;

namespace JL.Reminders.Core.Services
{
	public class RemindersService : IRemindersService
	{
		private readonly IRemindersRepository remindersRepository;
		private readonly IReminderCalculationService reminderCalculationService;

		public RemindersService(
			IRemindersRepository remindersRepository,
			IReminderCalculationService reminderCalculationService)
		{
			this.remindersRepository = remindersRepository;
			this.reminderCalculationService = reminderCalculationService;
		}

		public async Task<IEnumerable<Reminder>> GetRemindersByUserIdAsync(string userId)
		{
			var reminders = (await this.remindersRepository.GetRemindersByUserIdAsync(userId)).ToList();

			foreach (var reminder in reminders)
			{
				reminder.DaysToGo = reminderCalculationService.CalculateDaysToGo(reminder);
				reminder.SubTitle = GetReminderSubtitle(reminder);
			}

			return reminders;
		}

		public Task<ReminderOptions> GetReminderOptions()
		{
			ReminderOptions options = new ReminderOptions
			{
				Importances = EnumToDict<Importance>(),
				Recurrences = EnumToDict<Recurrence>()
			};

			return Task.FromResult(options);
		}

		public async Task<Reminder> GetReminderByIdAsync(string userId, long reminderId)
		{
			var reminder = await this.remindersRepository.GetReminderByIdAsync(userId, reminderId);
			reminder.DaysToGo = reminderCalculationService.CalculateDaysToGo(reminder);
			reminder.SubTitle = GetReminderSubtitle(reminder);
			return reminder;
		}

		public async Task<long> AddReminderAsync(string userId, Reminder reminder)
		{
			return await this.remindersRepository.AddReminderAsync(userId, reminder);
		}

		public async Task<bool> DeleteReminderAsync(string userId, long reminderId)
		{
			return await this.remindersRepository.DeleteReminderAsync(userId, reminderId);
		}

		public async Task<bool> UpdateReminderAsync(string userId, Reminder reminder)
		{
			return await this.remindersRepository.UpdateReminderAsync(userId, reminder);
		}

		public async Task<bool> ActionReminderAsync(string userId, ReminderAction action)
		{
			// TODO: temp code pending db re-schema to store the action timestamp and notes

			action.ActionedAt = DateTime.UtcNow;
			if (String.IsNullOrWhiteSpace(action.Notes))
			{
				action.Notes = null;
			}

			// get the next due date
			var reminder = await this.remindersRepository.GetReminderByIdAsync(userId, action.ReminderId);

			if (reminder == null)
			{
				return false;
			}

			var nextDue = this.reminderCalculationService.CalculateNextDueDate(reminder);

			// set it as the last-actioned date
			return await this.remindersRepository.UpdateReminderLastActionedAsync(userId, action.ReminderId, nextDue);
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

		/// <summary>
		/// <para>Utility method converts an enum type to a dictionary of integer value
		/// to member name, for all enum members.</para>
		/// 
		/// <para>If a member is decorated with a [<see cref="DescriptionAttribute"/>], the
		/// description is used instead of the member name.</para>
		/// </summary>
		/// <typeparam name="T">An enum type from which to generate a dictionary.</typeparam>
		/// <returns>A dictionary of int value -> name [or description] for all enum members.</returns>
		private static Dictionary<int, string> EnumToDict<T>() where T : struct, IConvertible
		{
			Dictionary<int, string> dict = new Dictionary<int, string>();

			var memberInfos = typeof(T).GetFields(BindingFlags.Static | BindingFlags.Public);

			foreach (var mi in memberInfos)
			{
				T theMember = (T)Enum.Parse(typeof(T), mi.Name);

				var attrs = mi.GetCustomAttributes(typeof(DescriptionAttribute), false)
					.Cast<DescriptionAttribute>()
					.ToList();

				dict.Add(
					Convert.ToInt32(theMember), attrs.Count > 0
						? attrs.First().Description
						: theMember.ToString(CultureInfo.InvariantCulture));
			}

			return dict;
		}
	}
}
