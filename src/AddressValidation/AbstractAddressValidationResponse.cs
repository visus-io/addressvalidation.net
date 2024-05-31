namespace AddressValidation;

using System.Collections.ObjectModel;
using Abstractions;
using FluentValidation;
using FluentValidation.Results;
using Http.Abstractions;

/// <inheritdoc />
public abstract class AbstractAddressValidationResponse : IAddressValidationResponse
{
	protected AbstractAddressValidationResponse(ValidationResult? validationResult)
	{
		if ( validationResult is null )
		{
			return;
		}

		Errors = validationResult.Errors
								 .Where(w => w.Severity == Severity.Error)
								 .Select(s => !string.IsNullOrWhiteSpace(s.ErrorCode) ? $"{s.ErrorCode}: {s.ErrorMessage}" : $"{s.ErrorMessage}")
								 .Distinct()
								 .ToList()
								 .AsReadOnly();

		Warnings = validationResult.Errors
								   .Where(w => w.Severity == Severity.Warning)
								   .Select(s => !string.IsNullOrWhiteSpace(s.ErrorCode) ? $"{s.ErrorCode}: {s.ErrorMessage}" : $"{s.ErrorMessage}")
								   .Distinct()
								   .ToList()
								   .AsReadOnly();
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
public abstract class AbstractAddressValidationResponse<TResponse> : AbstractAddressValidationResponse
	where TResponse : IApiAddressValidationResponse
{
	protected AbstractAddressValidationResponse(TResponse addressValidationResponse, ValidationResult? validationResult)
		: base(validationResult)
	{
	}
}

public abstract class AbstractAddressValidationResponse<TResponse, TSuggestion> : AbstractAddressValidationResponse<TResponse>
	where TResponse : IApiAddressValidationResponse
	where TSuggestion : class, new()
{
	private readonly List<IAddressValidationResponse> _suggestions = [];

	protected AbstractAddressValidationResponse(TResponse response, ValidationResult? validationResult)
		: base(response, validationResult)
	{
	}
}
