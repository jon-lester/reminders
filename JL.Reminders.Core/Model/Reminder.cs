using System;

namespace JL.Reminders.Core.Model
{
    public class Reminder
    {
		public long ID { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
	    public DateTime ForDate { get; set; }

		/// <summary>
		/// TODO: set this from a RemindersService
		/// </summary>
		public int DaysToGo { get; set; }
		public DateTime Created { get; set; }
		public Recurrence Recurrence { get; set; }
		public Importance Importance { get; set; }
	    public DateTime? LastActioned { get; set; }
	}
}