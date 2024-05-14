namespace AddressValidation.Google.Http;

using AddressValidation.Abstractions;

/// <inheritdoc />
public sealed class AddressValidationRequest : AddressValidationRequestBase
{
	private readonly HashSet<CountryCode> _cassEnabledCountries =
	[
		CountryCode.US,
		CountryCode.PR
	];

	/// <summary>
	///     Gets if USPS CASS processing is supported
	/// </summary>
	/// <remarks>Currently, USPS CASS is only supported for <see cref="CountryCode.US"/> and <see cref="CountryCode.PR"/>.</remarks>
	public bool EnableUspsCass => Country is not null && _cassEnabledCountries.Contains(Country.Value);
}
