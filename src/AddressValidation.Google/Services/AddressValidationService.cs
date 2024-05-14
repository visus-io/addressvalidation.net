namespace AddressValidation.Google.Services;

using System.Runtime.CompilerServices;
using Abstractions;
using AddressValidation.Abstractions;
using FluentValidation;
using FluentValidation.Results;
using Http;
using Microsoft.Extensions.Logging;
using Refit;

public sealed class AddressValidationService(
	IAddressValidationClient client,
	ILogger<AddressValidationService> logger,
	IValidator<AddressValidationRequest> validator) : IAddressValidationService<AddressValidationRequest>
{
	private readonly IAddressValidationClient _client = client ?? throw new ArgumentNullException(nameof(client));

	private readonly ILogger<AddressValidationService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

	private readonly IValidator<AddressValidationRequest> _validator = validator ?? throw new ArgumentNullException(nameof(validator));

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public ValueTask<IAddressValidationResponse?> ValidateAsync(AddressValidationRequest request, CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(request);
		return ValidateInternalAsync(request, cancellationToken);
	}

	private async ValueTask<IAddressValidationResponse?> ValidateInternalAsync(AddressValidationRequest request, CancellationToken cancellationToken)
	{
		ValidationResult? validationResult = await _validator.ValidateAsync(request, cancellationToken);
		if ( !validationResult.IsValid )
		{
			foreach ( ValidationFailure error in validationResult.Errors.Where(w => w is not null) )
			{
				_logger.LogError("{code}: {message}", error.ErrorCode, error.ErrorMessage);
			}

			return null;
		}

		ApiResponse<IAddressValidationResponse> response = await _client.ValidateAddressAsync(request, cancellationToken);

		return !response.IsSuccessStatusCode
				   ? null
				   : response.Content;
	}
}
