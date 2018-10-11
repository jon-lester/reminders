using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JL.Reminders.Api.Models
{
	public class PatchUserPreferencesModel
	{
		public int SoonDays { get; set; }
		public int ImminentDays { get; set; }
	}
}
