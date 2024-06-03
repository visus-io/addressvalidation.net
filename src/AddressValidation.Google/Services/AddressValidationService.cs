namespace AddressValidation.Google.Services;

using FluentValidation;
using Http;

internal sealed class AddressValidationService(
	AddressValidationClient client,
	IValidator<AddressValidationRequest> requestValidator,
	IValidator<ApiAddressValidationResponse> responseValidator)
	: AbstractAddressValidationService<AddressValidationRequest, ApiAddressValidationResponse>(requestValidator, responseValidator)
{
	private readonly AddressValidationClient _client = client ?? throw new ArgumentNullException(nameof(client));

	protected override async ValueTask<ApiAddressValidationResponse?> SendAsync(AddressValidationRequest request, CancellationToken cancellationToken)
	{
		return await _client.ValidateAddressAsync(request, cancellationToken);
	}
}
