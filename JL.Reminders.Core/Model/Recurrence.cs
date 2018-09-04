using System.ComponentModel;

namespace JL.Reminders.Core.Model
{
    public enum Recurrence
    {
		[Description("One-off")]
		OneOff = 0,
		[Description("Annual")]
		Annual = 1,
	    [Description("Six-Monthly")]
		SixMonthly = 2,
	    [Description("Quarterly")]
		Quarterly = 3,
	    [Description("Monthly")]
		Monthly = 4
    }
}
