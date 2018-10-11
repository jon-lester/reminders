using System;

using JL.Reminders.Core.Services.Interfaces;

namespace JL.Reminders.Core.Services
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
