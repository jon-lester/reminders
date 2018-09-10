using FluentValidation;

using JL.Reminders.Api.Models;

namespace JL.Reminders.Api.Validators
{
	// note - this class is auto-discovered so no reference shows
	public class PostNewReminderModelValidator : AbstractValidator<PostNewReminderModel>
    {
	    public PostNewReminderModelValidator()
	    {
		    RuleFor(m => m.Title).Length(1, 64).WithMessage($"{nameof(PostNewReminderModel.Title)} must be 1-64 characters.");
		    RuleFor(m => m.Importance).IsInEnum().WithMessage($"{nameof(PostNewReminderModel.Importance)} must be a valid reminder importance.");
		    RuleFor(m => m.Recurrence).IsInEnum().WithMessage($"{nameof(PostNewReminderModel.Recurrence)} must be a valid reminder recurrence type.");
		    RuleFor(m => m.ForDate).NotEmpty().WithMessage($"{nameof(PostNewReminderModel.ForDate)} must be a valid datetime.");
	    }
    }
}
