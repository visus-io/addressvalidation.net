namespace AddressValidation.Abstractions;

using FluentValidation;

/// <summary>
///     Base Validator for <see cref="IAddressValidationRequest" /> instances.
/// </summary>
public abstract class AddressValidationRequestAbstractValidator<T> : AbstractValidator<T>
	where T : IAddressValidationRequest
{
	/// <summary>
	///     Initializes a new instance of <see cref="AddressValidationRequestAbstractValidator{T}" />.
	/// </summary>
	/// <remarks>Contains conditional validation for countries that do not support a state/province.</remarks>
	protected AddressValidationRequestAbstractValidator()
	{
		RuleFor(r => r.AddressLines).NotEmpty();
		RuleFor(r => r.CityOrTown).NotNull().NotEmpty();
		RuleFor(r => r.Country).NotNull();
		RuleFor(r => r.PostalCode).NotNull().NotEmpty();

		RuleForEach(r => r.AddressLines).NotNull().NotEmpty();

		When(w => !AddressGlobals.CityStates.Contains(w.Country!.Value), () =>
																		 {
																			 RuleFor(r => r.StateOrProvince).NotNull().NotEmpty();
																		 });
	}
}
