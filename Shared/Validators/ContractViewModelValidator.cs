using FluentValidation;
using Shared.DTOs;

namespace Shared.Validators;

public class ContractViewModelValidator : AbstractValidator<ContractViewModel>
{
	public ContractViewModelValidator()
	{
		RuleFor(x => x.ContractTypeId)
			.GreaterThan(0)
			.WithMessage("Contract Type Id is required and must be greater than zero.");

		RuleFor(x => x.Description)
			.NotEmpty()
			.WithMessage("Description is required.");

		RuleFor(x => x.StartDate)
			.NotEmpty()
			.WithMessage("Start Date is required.");	

		RuleFor(x => x.Salary)
			.GreaterThan(0)
			.WithMessage("Salary must be greater than zero.");

		RuleFor(x => x.EmployeeEmployersId)
			.GreaterThan(0)
			.WithMessage("Employee Employers Id is required and must be greater than zero.");
	}

	public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
	{
		var result = await ValidateAsync(ValidationContext<ContractViewModel>.CreateWithOptions(
			(ContractViewModel)model, x => x.IncludeProperties(propertyName)));
		if (result.IsValid)
			return [];
		return result.Errors.Select(e => e.ErrorMessage);
	};
}
