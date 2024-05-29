namespace AddressValidation.Google.Abstractions;

using AddressValidation.Abstractions;
using Http;
using Refit;

internal interface IAddressValidationClient
{
	[Post("/v1:validateAddress")]
	Task<ApiResponse<ApiAddressValidationResponse>> ValidateAddressAsync([Body(true)] AddressValidationRequest abstractRequest, CancellationToken cancellationToken = default);
}
