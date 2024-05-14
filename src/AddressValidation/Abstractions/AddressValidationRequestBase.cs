namespace AddressValidation.Abstractions;

/// <inheritdoc />
public abstract class AddressValidationRequestBase : IAddressValidationRequest
{
	private CountryCode? _country;

	private string? _postalCode;

	private string? _stateOrProvince;

	/// <inheritdoc />
	public ISet<string> AddressLines { get; } = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

	/// <inheritdoc />
	public string? CityOrTown { get; set; }

	/// <inheritdoc />
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

			if ( AddressGlobals.NoPostalCodeCountries.Contains(value.Value) )
			{
				PostalCode = null;
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

			_postalCode = AddressGlobals.NoPostalCodeCountries.Contains(_country.Value)
							  ? null
							  : value;
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
