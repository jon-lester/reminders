using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using AutoMapper;

using JL.Reminders.Core.Entities;
using JL.Reminders.Core.Model;
using JL.Reminders.Core.Repositories;
using JL.Reminders.Core.Services.Interfaces;

namespace JL.Reminders.Core.Services
{
	public class RemindersService : IRemindersService
	{
		private readonly IRemindersRepository remindersRepository;
		private readonly IReminderHydrationService reminderHydrationService;

		public RemindersService(
			IRemindersRepository remindersRepository,
			IReminderHydrationService reminderHydrationService)
		{
			this.remindersRepository = remindersRepository;
			this.reminderHydrationService = reminderHydrationService;
		}

		public async Task<IEnumerable<Reminder>> GetRemindersByUserIdAsync(string userId, ReminderStatus status)
		{
			var reminders = (await this.remindersRepository.GetRemindersByUserIdAsync(userId, status)).ToList();

			return reminders.Select(r => reminderHydrationService.HydrateReminder(r)).ToList();
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

			return reminderHydrationService?.HydrateReminder(reminder);
		}

		public async Task<long> AddReminderAsync(string userId, Reminder reminder)
		{
			return await this.remindersRepository.AddReminderAsync(userId,
				Mapper.Map<Reminder, ReminderEntity>(reminder, opts => opts.ConfigureMap(MemberList.Destination)));
		}

		public async Task<bool> DeleteReminderAsync(string userId, long reminderId)
		{
			return await this.remindersRepository.DeleteReminderAsync(userId, reminderId);
		}

		public async Task<bool> UpdateReminderAsync(string userId, Reminder reminder)
		{
			return await this.remindersRepository.UpdateReminderAsync(userId,
				Mapper.Map<Reminder, ReminderEntity>(reminder, opts => opts.ConfigureMap(MemberList.Destination)));
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
			var reminder = await this.GetReminderByIdAsync(userId, action.ReminderId);

			if (reminder == null)
			{
				return false;
			}

			// set it as the last-actioned date
			return await this.remindersRepository.UpdateReminderLastActionedAsync(userId, reminder.Id, reminder.NextDueDate);
		}

		public async Task<bool> SetReminderStatusAsync(string userId, long reminderId, ReminderStatus status)
		{
			return await this.remindersRepository.SetReminderStatusAsync(userId, reminderId, status);
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
