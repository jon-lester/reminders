using System.Collections.Generic;

namespace JL.Reminders.Core.Model
{
	public class ReminderOptions
	{
		public Dictionary<int, string> Recurrences { get; set; }
		public Dictionary<int, string> Importances { get; set; }
	}
}
