namespace AddressValidation.Google.Services;

using Abstractions;
using FluentValidation;
using Http;
using Refit;

internal sealed class AddressValidationService(
	IAddressValidationClient client,
	IValidator<AddressValidationRequest> requestValidator,
	IValidator<ApiAddressValidationResponse> responseValidator)
	: AbstractAddressValidationService<AddressValidationRequest, ApiAddressValidationResponse>(requestValidator, responseValidator)
{
	private readonly IAddressValidationClient _client = client ?? throw new ArgumentNullException(nameof(client));

	protected override async ValueTask<ApiAddressValidationResponse?> SendAsync(AddressValidationRequest request, CancellationToken cancellationToken)
	{
		ApiResponse<ApiAddressValidationResponse> response = await _client.ValidateAddressAsync(request, cancellationToken);
		return !response.IsSuccessStatusCode ? null : response.Content;
	}
}
