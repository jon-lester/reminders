using System;

namespace JL.Reminders.Core.Model
{
	public class ReminderAction
	{
		public long ReminderId { get; set; }
		public DateTime ActionedAt { get; set; }
		public string Notes { get; set; }
	}
}
