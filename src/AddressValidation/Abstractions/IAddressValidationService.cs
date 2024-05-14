namespace AddressValidation.Abstractions;

public interface IAddressValidationService<in TRequest>
	where TRequest : IAddressValidationRequest
{
	ValueTask<IAddressValidationResponse?> ValidateAsync(TRequest request, CancellationToken cancellationToken = default);
}
