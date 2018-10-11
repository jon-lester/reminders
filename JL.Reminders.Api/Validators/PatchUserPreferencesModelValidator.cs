using System;
using FluentValidation;

using JL.Reminders.Api.Models;

namespace JL.Reminders.Api.Validators
{
	public class PatchUserPreferencesModelValidator : AbstractValidator<PatchUserPreferencesModel>
	{
		public PatchUserPreferencesModelValidator()
		{
			RuleFor(m => m.SoonDays)
				.InclusiveBetween(1, 365)
				.WithMessage($"{nameof(PatchUserPreferencesModel.SoonDays)} must be between 1 and 365.");

			RuleFor(m => m.ImminentDays)
				.InclusiveBetween(1, 365)
				.WithMessage($"{nameof(PatchUserPreferencesModel.ImminentDays)} must be between 1 and 365.");

			RuleFor(m => m.SoonDays)
				.GreaterThan(m => m.ImminentDays)
				.WithMessage($"{nameof(PatchUserPreferencesModel.SoonDays)} must be greater than {nameof(PatchUserPreferencesModel.ImminentDays)}");
		}
	}
}