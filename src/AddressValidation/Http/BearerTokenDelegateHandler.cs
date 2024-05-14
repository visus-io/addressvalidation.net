namespace AddressValidation.Http;

using System.Net.Http.Headers;
using System.Security.Authentication;
using Abstractions;

/// <summary>
///     An HTTP handler delegate that injects a Bearer token into each request.
/// </summary>
/// <param name="authenticationService">An instance of <see cref="IAuthenticationService" />.</param>
public sealed class BearerTokenDelegateHandler(IAuthenticationService authenticationService) : DelegatingHandler
{
	private readonly IAuthenticationService _authenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));

	/// <inheritdoc />
	/// <exception cref="InvalidCredentialException">Provided credentials were rejected by the server.</exception>
	protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		ArgumentNullException.ThrowIfNull(request);

		if ( request.RequestUri is null )
		{
			return await base.SendAsync(request, cancellationToken);
		}

		string? accessToken = await _authenticationService.GetAccessTokenAsync(cancellationToken);
		if ( string.IsNullOrWhiteSpace(accessToken) )
		{
			throw new InvalidCredentialException();
		}

		request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

		return await base.SendAsync(request, cancellationToken);
	}
}
