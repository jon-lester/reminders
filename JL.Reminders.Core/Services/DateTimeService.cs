using System;

namespace JL.Reminders.Core.Services
{
	public class DateTimeService : IDateTimeService
	{
		public DateTime GetCurrentDateTime()
		{
			return DateTime.UtcNow;
		}
	}
}
