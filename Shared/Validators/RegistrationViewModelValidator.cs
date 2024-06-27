using FluentValidation;
using Shared.DTOs;

namespace Shared.Validators;

public class RegistrationViewModelValidator : AbstractValidator<RegistrationViewModel>
{
	public RegistrationViewModelValidator()
	{
		RuleFor(x => x.Email)
			.NotEmpty().WithMessage("Email is required.")
			.EmailAddress().WithMessage("A valid email is required.");

		RuleFor(x => x.Password)
			.NotEmpty().WithMessage("Password is required.")
			.MinimumLength(6).WithMessage("Password must be at least 6 characters long.");

		RuleFor(x => x.ConfirmPassword)
			.NotEmpty().WithMessage("Confirm Password is required.")
			.Equal(x => x.Password).WithMessage("Passwords do not match.");

		RuleFor(x => x.FirstName)
			.NotEmpty().WithMessage("First Name is required.");

		RuleFor(x => x.LastName)
			.NotEmpty().WithMessage("Last Name is required.");
	}
}
