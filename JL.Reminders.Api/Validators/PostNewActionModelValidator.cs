using FluentValidation;

using JL.Reminders.Api.Models;

namespace JL.Reminders.Api.Validators
{
	public class PostNewActionModelValidator : AbstractValidator<PostNewActionModel>
	{
		public PostNewActionModelValidator()
		{
			RuleFor(m => m.ReminderId)
				.GreaterThan(0)
				.WithMessage($"{nameof(PostNewActionModel.ReminderId)} must be a valid reminder ID.");
		}
	}
}