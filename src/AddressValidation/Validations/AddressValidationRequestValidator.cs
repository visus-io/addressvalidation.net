namespace AddressValidation.Validations;

using Abstractions;
using FluentValidation;

/// <summary>
///     Validator for <see cref="IAddressValidationRequest" /> instances.
/// </summary>
public sealed class AddressValidationRequestValidator : AbstractValidator<IAddressValidationRequest>
{
	/// <summary>
	///     Initializes a new instance of <see cref="AddressValidationRequestValidator" />.
	/// </summary>
	/// <remarks>Contains conditional validation for countries that do not support a state/province and/or postal code.</remarks>
	public AddressValidationRequestValidator()
	{
		RuleFor(r => r.AddressLines).NotEmpty();
		RuleFor(r => r.CityOrTown).NotNull().NotEmpty();
		RuleFor(r => r.Country).NotNull();

		RuleForEach(r => r.AddressLines).NotNull().NotEmpty();

		When(w => !AddressGlobals.CityStates.Contains(w.Country!.Value), () =>
																			 {
																				 RuleFor(r => r.StateOrProvince).NotNull().NotEmpty();
																			 });

		When(w => !AddressGlobals.NoPostalCodeCountries.Contains(w.Country!.Value), () =>
																						{
																							RuleFor(r => r.PostalCode).NotNull().NotEmpty();
																						});
	}
}
