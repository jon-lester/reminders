using System;

using JL.Reminders.Core.Services;

namespace JL.Reminders.Services
{
	public class DateTimeService : IDateTimeService
	{
		public DateTime GetCurrentDateTime()
		{
			return DateTime.UtcNow;
		}

		public DateTime GetCurrentDate()
		{
			return GetCurrentDateTime().Date;
		}
	}
}
