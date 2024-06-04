namespace AddressValidation.Model;

using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using Abstractions;
using FluentValidation;
using FluentValidation.Results;

/// <summary>
///     Represents an empty validation response
/// </summary>
[ExcludeFromCodeCoverage]
public sealed class EmptyAddressValidationResponse : IAddressValidationResponse
{
	/// <summary>
	///     Initializes a new instance of the <see cref="EmptyAddressValidationResponse" /> class.
	/// </summary>
	/// <param name="validationResult">
	///     Current validation state of the response represented as an instance of
	///     <see cref="ValidationResult" />.
	/// </param>
	public EmptyAddressValidationResponse(ValidationResult? validationResult)
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
	public IReadOnlyCollection<string> AddressLines => ReadOnlyCollection<string>.Empty;

	/// <inheritdoc />
	public string? CityOrTown => null;

	/// <inheritdoc />
	public CountryCode Country => CountryCode.ZZ;

	/// <inheritdoc />
	public IReadOnlyCollection<string> Errors { get; } = ReadOnlyObservableCollection<string>.Empty;

	/// <inheritdoc />
	public string? PostalCode => null;

	/// <inheritdoc />
	public string? StateOrProvince => null;

	/// <inheritdoc />
	public IReadOnlyCollection<IAddressValidationResponse> Suggestions => ReadOnlyCollection<IAddressValidationResponse>.Empty;

	/// <inheritdoc />
	public IReadOnlyCollection<string> Warnings { get; } = ReadOnlyObservableCollection<string>.Empty;
}
