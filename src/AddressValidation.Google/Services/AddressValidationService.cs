namespace AddressValidation.Google.Services;

using Abstractions;
using AddressValidation.Abstractions;
using FluentValidation;
using FluentValidation.Results;
using Http;
using Model;
using Refit;

internal sealed class AddressValidationService(
	IAddressValidationClient client,
	IValidator<AddressValidationRequest> requestValidator,
	IValidator<ApiAddressValidationResponse> responseValidator)
	: AddressValidationAbstractService<AddressValidationRequest, ApiAddressValidationResponse>(requestValidator, responseValidator)
{
	private readonly IAddressValidationClient _client = client ?? throw new ArgumentNullException(nameof(client));

	protected override ValueTask<IAddressValidationResponse?> ConvertResponseAsync(ApiAddressValidationResponse response, ValidationResult? responseValidationResult, CancellationToken cancellationToken)
	{
		AddressValidationResponse model = new(response, responseValidationResult);
		return new ValueTask<IAddressValidationResponse?>(model);
	}

	protected override async ValueTask<ApiAddressValidationResponse?> SendRequestAsync(AddressValidationRequest request, CancellationToken cancellationToken)
	{
		ApiResponse<ApiAddressValidationResponse> response = await _client.ValidateAddressAsync(request, cancellationToken);
		return !response.IsSuccessStatusCode ? null : response.Content;
	}
}
