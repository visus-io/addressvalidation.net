namespace AddressValidation.Model;

using System.Collections.ObjectModel;
using Abstractions;
using FluentValidation;
using FluentValidation.Results;

/// <inheritdoc />
public sealed class EmptyAddressValidationResponse : IAddressValidationResponse
{
	public EmptyAddressValidationResponse(ValidationResult? validationResults)
	{
		if ( validationResults is null )
		{
			return;
		}

		Errors = validationResults.Errors
								  .Where(w => w.Severity == Severity.Error)
								  .Select(s => !string.IsNullOrWhiteSpace(s.ErrorCode) ? $"{s.ErrorCode}: {s.ErrorMessage}" : $"{s.ErrorMessage}")
								  .ToHashSet(StringComparer.OrdinalIgnoreCase);

		Warnings = validationResults.Errors
									.Where(w => w.Severity == Severity.Warning)
									.Select(s => !string.IsNullOrWhiteSpace(s.ErrorCode) ? $"{s.ErrorCode}: {s.ErrorMessage}" : $"{s.ErrorMessage}")
									.ToHashSet(StringComparer.OrdinalIgnoreCase);
	}

	/// <inheritdoc />
	public IReadOnlyCollection<string> AddressLines => ReadOnlyCollection<string>.Empty;

	/// <inheritdoc />
	public string? CityOrTown => null;

	/// <inheritdoc />
	public CountryCode Country => CountryCode.ZZ;

	/// <inheritdoc />
	public IReadOnlyCollection<string> Errors { get; }

	/// <inheritdoc />
	public string? PostalCode => null;

	/// <inheritdoc />
	public string? StateOrProvince => null;

	/// <inheritdoc />
	public IReadOnlyCollection<IAddressValidationResponse> Suggestions => ReadOnlyCollection<IAddressValidationResponse>.Empty;

	/// <inheritdoc />
	public IReadOnlyCollection<string> Warnings { get; }
}
