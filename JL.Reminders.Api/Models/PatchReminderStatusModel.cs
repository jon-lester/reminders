using System;

using JL.Reminders.Core.Model;

namespace JL.Reminders.Api.Models
{
	public class PatchReminderStatusModel
	{
		public ReminderStatus Status { get; set; }
	}
}
