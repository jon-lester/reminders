using System;

namespace JL.Reminders.Core.Services.Interfaces
{
	public interface IDateTimeService
	{
		DateTime GetCurrentDateTime();
		DateTime GetCurrentDate();
	}
}
