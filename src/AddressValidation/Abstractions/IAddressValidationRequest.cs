namespace AddressValidation.Abstractions;

using AddressValidation.Validations;

/// <summary>
///     Represents a uniformed address validation request.
/// </summary>
public interface IAddressValidationRequest
{
	/// <summary>
	///     Gets or sets address lines
	/// </summary>
	public ISet<string> AddressLines { get; }

	/// <summary>
	///     Gets or sets the city (town)
	/// </summary>
	public string? CityOrTown { get; set; }

	/// <summary>
	///     Gets or sets the country code
	/// </summary>
	/// <remarks>Refer to <see cref="Country" /> for values.</remarks>
	public CountryCode? Country { get; set; }

	/// <summary>
	///     Gets or sets the residential status
	/// </summary>
	/// <remarks>Value may be ignored for some validation services.</remarks>
	public bool? IsResidential { get; set; }

	/// <summary>
	///     Gets or sets the Zip (Postal) Code
	/// </summary>
	/// <remarks>Value will be dropped for countries that have no concept of a postal code. See <see cref="AddressValidationRequestValidator"/> for details.</remarks>
	public string? PostalCode { get; set; }

	/// <summary>
	///     Gets or sets the State (Province)
	/// </summary>
	/// <remarks>Value will be dropped for countries that are considered city-states. See <see cref="AddressValidationRequestValidator"/> for details.</remarks>
	public string? StateOrProvince { get; set; }
}
