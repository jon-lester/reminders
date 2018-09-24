using FluentValidation;

using JL.Reminders.Api.Models;

namespace JL.Reminders.Api.Validators
{
	public class PatchReminderStatusModelValidator : AbstractValidator<PatchReminderStatusModel>
	{
		public PatchReminderStatusModelValidator()
		{
			RuleFor(m => m.Status)
				.IsInEnum()
				.WithMessage($"{nameof(PatchReminderStatusModel.Status)} must be a valid reminder status.");
		}
	}
}
