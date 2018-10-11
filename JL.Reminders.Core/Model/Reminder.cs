using System;

namespace JL.Reminders.Core.Model
{
    public class Reminder
    {
	    public long Id { get; set; }
	    public string Title { get; set; }
	    public string SubTitle { get; set; }
		public string Description { get; set; }
		public ReminderStatus Status { get; set; }

	    public DateTime ForDate { get; set; }
	    public DateTime Created { get; set; }
	    public DateTime? LastActioned { get; set; }
	    public DateTime NextDueDate { get; set; }
	    public int DaysToGo { get; set; }

		public Recurrence Recurrence { get; set; }
	    public Importance Importance { get; set; }

	    public int? SoonDaysPreference { get; set; }
	    public int? ImminentDaysPreference { get; set; }
	}
}