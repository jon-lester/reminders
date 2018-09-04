using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using FluentValidation;
using JL.Reminders.Api.Models;

namespace JL.Reminders.Api.Validators
{
    public class ReminderDetailsModelValidator : AbstractValidator<ReminderCreateModel>
    {
	    public ReminderDetailsModelValidator()
	    {
		    RuleFor(m => m.Title).Length(1, 64).WithMessage($"{nameof(ReminderCreateModel.Title)} must be 1-64 characters.");
		    RuleFor(m => m.Importance).IsInEnum().WithMessage($"{nameof(ReminderCreateModel.Importance)} must be a valid reminder importance.");
		    RuleFor(m => m.Recurrence).IsInEnum().WithMessage($"{nameof(ReminderCreateModel.Recurrence)} must be a valid reminder recurrence type.");
		    RuleFor(m => m.ForDate).NotEmpty().WithMessage($"{nameof(ReminderCreateModel.ForDate)} must be a valid datetime.");
	    }
    }
}
