namespace AddressValidation.Google.Abstractions;

using AddressValidation.Abstractions;
using Http;
using Refit;

public interface IAddressValidationClient
{
	[Post("/v1:validateAddress")]
	Task<ApiResponse<IAddressValidationResponse>> ValidateAddressAsync([Body(true)] AddressValidationRequest request, CancellationToken cancellationToken = default);
}
