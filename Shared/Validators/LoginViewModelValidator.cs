using FluentValidation;
using Shared.DTOs;

namespace Shared.Validators;

public class LoginViewModelValidator : AbstractValidator<LoginViewModel>
{
	public LoginViewModelValidator()
	{
		RuleFor(x => x.Email)
			.NotEmpty().WithMessage("Email is required.")
			.EmailAddress().WithMessage("A valid email is required.");

		RuleFor(x => x.Password)
			.NotEmpty().WithMessage("Password is required.")
			.MinimumLength(6).WithMessage("Password must be at least 6 characters long.");
	}
}
