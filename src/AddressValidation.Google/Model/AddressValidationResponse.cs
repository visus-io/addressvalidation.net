namespace AddressValidation.Google.Model;

using AddressValidation.Abstractions;
using FluentValidation.Results;
using Http;

internal sealed class AddressValidationResponse : AddressValidationAbstractResponse<ApiAddressValidationResponse>
{
	public AddressValidationResponse(ApiAddressValidationResponse response, ValidationResult? validationResult)
		: base(response, validationResult)
	{
		AddressLines = response.Result.Address.PostalAddress.AddressLines;
		CityOrTown = response.Result.Address.PostalAddress.Locality;
		Country = response.Result.Address.PostalAddress.RegionCode;
		PostalCode = response.Result.Address.PostalAddress.PostalCode;
		StateOrProvince = response.Result.Address.PostalAddress.AdministrativeArea;
	}
}
