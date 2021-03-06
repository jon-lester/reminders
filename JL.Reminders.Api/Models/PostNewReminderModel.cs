﻿using System;

using JL.Reminders.Core.Model;

namespace JL.Reminders.Api.Models
{
    public class PostNewReminderModel
    {
	    public string Title { get; set; }
	    public string Description { get; set; }
	    public DateTime ForDate { get; set; }
	    public Recurrence Recurrence { get; set; }
	    public Importance Importance { get; set; }
	}
}
