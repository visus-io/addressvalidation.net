namespace AddressValidation.Abstractions;

/// <inheritdoc />
public abstract class AddressValidationAbstractResponse : IAddressValidationResponse
{
	private readonly HashSet<string> _errors = new(StringComparer.OrdinalIgnoreCase);

	/// <inheritdoc />
	public IReadOnlyCollection<string> Errors => _errors;

	/// <inheritdoc />
	public IReadOnlyCollection<string> AddressLines { get; protected init; } = [];

	/// <inheritdoc />
	public string? CityOrTown { get; protected init; }

	/// <inheritdoc />
	public CountryCode Country { get; protected init; }

	/// <inheritdoc />
	public string? PostalCode { get; protected init; }

	/// <inheritdoc />
	public string? StateOrProvince { get; protected init; }

	/// <inheritdoc />
	public IReadOnlyCollection<IAddressValidationResponse> Suggestions { get; protected set; } = [];
}

/// <inheritdoc />
public abstract class AddressValidationAbstractResponse<TResponse> : AddressValidationAbstractResponse
	where TResponse : class, new()
{
	protected AddressValidationAbstractResponse(TResponse response)
	{
	}
}

public abstract class AddressValidationAbstractResponse<TResponse, TSuggestion> : AddressValidationAbstractResponse<TResponse>
	where TResponse : class, new()
	where TSuggestion : class, new()
{
	private readonly List<IAddressValidationResponse> _suggestions = [];

	protected AddressValidationAbstractResponse(TResponse response)
		: base(response)
	{
	}
}
