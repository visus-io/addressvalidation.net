namespace AddressValidation.Abstractions;

using System.Text.Json.Serialization;

/// <inheritdoc />
public abstract class AddressValidationAbstractRequest : IAddressValidationRequest
{
	private CountryCode? _country;

	private string? _postalCode;

	private string? _stateOrProvince;

	/// <inheritdoc />
	public ISet<string> AddressLines { get; } = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

	/// <inheritdoc />
	public string? CityOrTown { get; set; }

	/// <inheritdoc />
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public CountryCode? Country
	{
		get => _country;
		set
		{
			_country = value;

			if ( value is null )
			{
				return;
			}

			if ( AddressGlobals.CityStates.Contains(value.Value) )
			{
				StateOrProvince = null;
			}
		}
	}

	/// <inheritdoc />
	public bool? IsResidential { get; set; }

	/// <inheritdoc />
	public string? PostalCode
	{
		get => _postalCode;
		set
		{
			if ( _country is null )
			{
				_postalCode = value;
				return;
			}

			_postalCode = value;
		}
	}

	/// <inheritdoc />
	public string? StateOrProvince
	{
		get => _stateOrProvince;
		set
		{
			if ( _country is null )
			{
				_stateOrProvince = value;
				return;
			}

			_stateOrProvince = AddressGlobals.CityStates.Contains(_country.Value)
								   ? null
								   : value;
		}
	}
}
