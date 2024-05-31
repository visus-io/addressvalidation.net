namespace AddressValidation.Http.Abstractions;

using AddressValidation.Abstractions;
using FluentValidation.Results;

public interface IApiAddressValidationResponse
{
	public Guid Id { get; }

	public IAddressValidationResponse ToAddressValidationResponse(ValidationResult? validationResult);
}
