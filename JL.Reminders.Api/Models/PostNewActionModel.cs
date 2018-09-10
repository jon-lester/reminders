using System;

namespace JL.Reminders.Api.Models
{
	public class PostNewActionModel
	{
		public long ReminderId { get; set; }
		public string Notes { get; set; }
	}
}
