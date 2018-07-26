using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using FluentValidation;
using JL.Reminders.Api.Models;

namespace JL.Reminders.Api.Validators
{
    public class ReminderDetailsModelValidator : AbstractValidator<ReminderDetailsModel>
    {
	    public ReminderDetailsModelValidator()
	    {
		    RuleFor(m => m.Title).Length(1, 64).WithMessage($"{nameof(ReminderDetailsModel.Title)} must be 1-64 characters.");
		    RuleFor(m => m.Importance).IsInEnum().WithMessage($"{nameof(ReminderDetailsModel.Importance)} must be a valid reminder importance.");
		    RuleFor(m => m.Recurrence).IsInEnum().WithMessage($"{nameof(ReminderDetailsModel.Recurrence)} must be a valid reminder recurrence type.");
		    RuleFor(m => m.ForDate).NotEmpty().WithMessage($"{nameof(ReminderDetailsModel.ForDate)} must be a valid datetime.");
	    }
    }
}
