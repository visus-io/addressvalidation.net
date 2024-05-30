namespace AddressValidation.Abstractions;

using System.Collections.ObjectModel;
using FluentValidation;
using FluentValidation.Results;

/// <inheritdoc />
public abstract class AddressValidationAbstractResponse : IAddressValidationResponse
{
	protected AddressValidationAbstractResponse(ValidationResult? validationResult)
	{
		if ( validationResult is null )
		{
			return;
		}

		Errors = validationResult.Errors
								 .Where(w => w.Severity == Severity.Error)
								 .Select(s => !string.IsNullOrWhiteSpace(s.ErrorCode) ? $"{s.ErrorCode}: {s.ErrorMessage}" : $"{s.ErrorMessage}")
								 .ToHashSet(StringComparer.OrdinalIgnoreCase);

		Warnings = validationResult.Errors
								   .Where(w => w.Severity == Severity.Warning)
								   .Select(s => !string.IsNullOrWhiteSpace(s.ErrorCode) ? $"{s.ErrorCode}: {s.ErrorMessage}" : $"{s.ErrorMessage}")
								   .ToHashSet(StringComparer.OrdinalIgnoreCase);
	}

	/// <inheritdoc />
	public IReadOnlyCollection<string> Errors { get; } = ReadOnlyCollection<string>.Empty;

	/// <inheritdoc />
	public IReadOnlyCollection<string> Warnings { get; } = ReadOnlyCollection<string>.Empty;

	/// <inheritdoc />
	public IReadOnlyCollection<string> AddressLines { get; protected init; } = ReadOnlyCollection<string>.Empty;

	/// <inheritdoc />
	public string? CityOrTown { get; protected init; }

	/// <inheritdoc />
	public CountryCode Country { get; protected init; }

	/// <inheritdoc />
	public string? PostalCode { get; protected init; }

	/// <inheritdoc />
	public string? StateOrProvince { get; protected init; }

	/// <inheritdoc />
	public IReadOnlyCollection<IAddressValidationResponse> Suggestions { get; protected set; } = ReadOnlyCollection<IAddressValidationResponse>.Empty;
}

/// <inheritdoc />
public abstract class AddressValidationAbstractResponse<TResponse> : AddressValidationAbstractResponse
	where TResponse : IApiResponse, new()
{
	protected AddressValidationAbstractResponse(TResponse response, ValidationResult? validationResult)
		: base(validationResult)
	{
	}
}

public abstract class AddressValidationAbstractResponse<TResponse, TSuggestion> : AddressValidationAbstractResponse<TResponse>
	where TResponse : IApiResponse, new()
	where TSuggestion : class, new()
{
	private readonly List<IAddressValidationResponse> _suggestions = [];

	protected AddressValidationAbstractResponse(TResponse response, ValidationResult? validationResult)
		: base(response, validationResult)
	{
	}
}
