namespace AddressValidation.Abstractions;

public interface IAuthenticationService
{
	ValueTask<string?> GetAccessTokenAsync(CancellationToken cancellationToken = default);
}
