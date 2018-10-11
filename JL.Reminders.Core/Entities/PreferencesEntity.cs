using System;
using System.Collections.Generic;
using System.Text;

namespace JL.Reminders.Core.Entities
{
	public class PreferencesEntity
	{
		public long ID { get; set; }
		public string UserID { get; set; }
		public int SoonDays { get; set; }
		public int ImminentDays { get; set; }
	}
}
