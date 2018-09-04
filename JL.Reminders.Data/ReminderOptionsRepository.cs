using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using JL.Reminders.Core.Model;
using JL.Reminders.Core.Repositories;

namespace JL.Reminders.Data
{
	public class ReminderOptionsRepository : IReminderOptionsRepository
	{
		public Task<ReminderOptions> GetReminderOptions()
		{
			ReminderOptions options = new ReminderOptions
			{
				Importances = EnumToDict<Importance>(),
				Recurrences = EnumToDict<Recurrence>()
			};

			return Task.FromResult(options);
		}

		public static Dictionary<int, string> EnumToDict<T>() where T : struct, IConvertible
		{
			Dictionary<int, string> dict = new Dictionary<int, string>();

			var memberInfos = typeof(T).GetFields(BindingFlags.Static | BindingFlags.Public);

			foreach (var mi in memberInfos)
			{
				T theMember = (T) Enum.Parse(typeof(T), mi.Name);

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
