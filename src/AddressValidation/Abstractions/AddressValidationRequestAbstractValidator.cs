namespace AddressValidation.Abstractions;

using System.Diagnostics;
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

		When(w => w.Country is not null && !AddressGlobals.CityStates.Contains(w.Country.Value),
			 () =>
			 {
				 RuleFor(r => r.StateOrProvince).NotNull().NotEmpty();
			 });
		
		When(w => w.Country is not null,
			 () =>
			 {
				 RuleFor(r => r.Country)
					.Must(m =>
						  {
							  Debug.Assert(m is not null, nameof(m) + " is not null");
							  return !AddressGlobals.PostalCodeNotSupported.Contains(m.Value);
						  })
					.WithMessage("The country '{PropertyValue}' is currently not supported for address validation.");
			 });
	}
}
