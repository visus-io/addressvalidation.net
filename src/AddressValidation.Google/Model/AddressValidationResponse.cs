namespace AddressValidation.Google.Model;

using FluentValidation.Results;
using Http;

internal sealed class AddressValidationResponse : AbstractAddressValidationResponse<ApiAddressValidationResponse>
{
	public AddressValidationResponse(ApiAddressValidationResponse addressValidationResponse, ValidationResult? validationResult)
		: base(addressValidationResponse, validationResult)
	{
		AddressLines = addressValidationResponse.Result.Address.PostalAddress.AddressLines;
		CityOrTown = addressValidationResponse.Result.Address.PostalAddress.Locality;
		Country = addressValidationResponse.Result.Address.PostalAddress.RegionCode;
		PostalCode = addressValidationResponse.Result.Address.PostalAddress.PostalCode;
		StateOrProvince = addressValidationResponse.Result.Address.PostalAddress.AdministrativeArea;
	}
}
