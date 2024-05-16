namespace AddressValidation.Abstractions;

using System.Diagnostics;
using FluentValidation;

/// <summary>
///     Base Validator for <see cref="IAddressValidationRequest" /> instances.
/// </summary>
public abstract class AddressValidationRequestAbstractValidator<T> : AbstractValidator<T>
	where T : IAddressValidationRequest
{
	private readonly IReadOnlySet<CountryCode> _unsupportedCountries = new HashSet<CountryCode>
	{
		CountryCode.AE,
		CountryCode.AG,
		CountryCode.AO,
		CountryCode.AW,
		CountryCode.BF,
		CountryCode.BI,
		CountryCode.BJ,
		CountryCode.BM,
		CountryCode.BO,
		CountryCode.BQ,
		CountryCode.BS,
		CountryCode.BW,
		CountryCode.BZ,
		CountryCode.CD,
		CountryCode.CF,
		CountryCode.CG,
		CountryCode.CI,
		CountryCode.CK,
		CountryCode.CM,
		CountryCode.CW,
		CountryCode.DJ,
		CountryCode.DM,
		CountryCode.ER,
		CountryCode.FJ,
		CountryCode.GA,
		CountryCode.GD,
		CountryCode.GH,
		CountryCode.GM,
		CountryCode.GM,
		CountryCode.GQ,
		CountryCode.GY,
		CountryCode.HK,
		CountryCode.HM,
		CountryCode.KI,
		CountryCode.KM,
		CountryCode.KN,
		CountryCode.KP,
		CountryCode.LY,
		CountryCode.LY,
		CountryCode.ML,
		CountryCode.MO,
		CountryCode.MR,
		CountryCode.MW,
		CountryCode.NL,
		CountryCode.NR,
		CountryCode.NU,
		CountryCode.QA,
		CountryCode.RW,
		CountryCode.SB,
		CountryCode.SC,
		CountryCode.SL,
		CountryCode.SO,
		CountryCode.SR,
		CountryCode.ST,
		CountryCode.SX,
		CountryCode.SY,
		CountryCode.TD,
		CountryCode.TF,
		CountryCode.TG,
		CountryCode.TK,
		CountryCode.TL,
		CountryCode.TO,
		CountryCode.TT,
		CountryCode.TV,
		CountryCode.UG,
		CountryCode.VU,
		CountryCode.YE,
		CountryCode.ZW
	};

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
							  return !_unsupportedCountries.Contains(m.Value);
						  })
					.WithMessage("The country '{PropertyValue}' is currently not supported for address validation.");
			 });
	}
}
