namespace AddressValidation.Google.Services;

using System.Runtime.CompilerServices;
using Abstractions;
using AddressValidation.Abstractions;
using FluentValidation;
using FluentValidation.Results;
using Http;
using Microsoft.Extensions.Logging;
using Model;
using Refit;

internal sealed class AddressValidationService(
	IAddressValidationClient client,
	ILogger<AddressValidationService> logger,
	IValidator<AddressValidationRequest> validator) : IAddressValidationService<AddressValidationRequest>
{
	private readonly IAddressValidationClient _client = client ?? throw new ArgumentNullException(nameof(client));

	private readonly ILogger<AddressValidationService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

	private readonly IValidator<AddressValidationRequest> _validator = validator ?? throw new ArgumentNullException(nameof(validator));

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public ValueTask<IAddressValidationResponse?> ValidateAsync(AddressValidationRequest abstractRequest, CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(abstractRequest);
		return ValidateInternalAsync(abstractRequest, cancellationToken);
	}

	private async ValueTask<IAddressValidationResponse?> ValidateInternalAsync(AddressValidationRequest abstractRequest, CancellationToken cancellationToken)
	{
		ValidationResult? validationResult = await _validator.ValidateAsync(abstractRequest, cancellationToken);
		if ( !validationResult.IsValid )
		{
			foreach ( ValidationFailure? error in validationResult.Errors.Where(w => w is not null) )
			{
				_logger.LogError("{code}: {message}", error.ErrorCode, error.ErrorMessage);
			}

			return null;
		}

		ApiResponse<ApiAddressValidationResponse> apiResponse = await _client.ValidateAddressAsync(abstractRequest, cancellationToken);
		if ( !apiResponse.IsSuccessStatusCode )
		{
			return null;
		}

		AddressValidationResponse response = new(apiResponse.Content);

		return response;
	}
}
