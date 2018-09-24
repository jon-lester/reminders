using System;

using JL.Reminders.Core.Model;

namespace JL.Reminders.Core.Entities
{
	public class ReminderEntity
	{
		public long Id { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public DateTime ForDate { get; set; }
		public DateTime Created { get; set; }
		public Recurrence Recurrence { get; set; }
		public Importance Importance { get; set; }
		public DateTime? LastActioned { get; set; }
		public int? SoonDaysPreference { get; set; }
		public int? ImminentDaysPreference { get; set; }
		public ReminderStatus Status { get; set; }
	}
}
