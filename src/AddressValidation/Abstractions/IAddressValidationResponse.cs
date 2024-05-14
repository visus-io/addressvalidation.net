namespace AddressValidation.Abstractions;

/// <summary>
/// Represents a uniformed address validation response.
/// </summary>
public interface IAddressValidationResponse
{
	public IReadOnlyCollection<string> AddressLines { get; }

	/// <summary>
	/// Gets the city (town)
	/// </summary>
	public string? CityOrTown { get; }

	/// <summary>
	/// Gets the country code
	/// </summary>
	/// <remarks>Refer to <see cref="CountryCode" /> for values.</remarks>
	public CountryCode? CountryCode { get; }

	public bool? IsResdential { get; }

	/// <summary>
	/// Gets the zip (postal) code
	/// </summary>
	/// <remarks>Value may be omitted for countries that do not support the concept of a postal code.</remarks>
	public string? PostalCode { get; }

	/// <summary>
	/// Gets the state (province)
	/// </summary>
	/// <remarks>Value may be omitted for countries that are considered city-states.</remarks>
	public string? StateOrProvince { get; }
}
