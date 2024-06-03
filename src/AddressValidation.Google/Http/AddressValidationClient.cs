namespace AddressValidation.Google.Http;

using System.Net.Http.Json;
using Microsoft.Extensions.Logging;

internal sealed class AddressValidationClient
{
	private static readonly Uri GoogleAddressValidationBaseUri = new("https://addressvalidation.googleapis.com");

	private readonly HttpClient _httpClient;

	private readonly ILogger<AddressValidationClient> _logger;

	public AddressValidationClient(HttpClient httpClient, ILogger<AddressValidationClient> logger)
	{
		_httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
		_logger = logger ?? throw new ArgumentNullException(nameof(logger));

		_httpClient.BaseAddress = GoogleAddressValidationBaseUri;
	}

	public ValueTask<ApiAddressValidationResponse?> ValidateAddressAsync(GoogleAddressValidationRequest request, CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(request);
		return ValidateAddressInternalAsync(request, cancellationToken);
	}

	private async ValueTask<ApiAddressValidationResponse?> ValidateAddressInternalAsync(GoogleAddressValidationRequest request, CancellationToken cancellationToken)
	{
		HttpResponseMessage response = await _httpClient.PostAsJsonAsync("v1:validateAddress", request, cancellationToken);
		if ( response.IsSuccessStatusCode )
		{
			return await response.Content.ReadFromJsonAsync<ApiAddressValidationResponse>(cancellationToken);
		}

		ApiErrorResponse? errorResponse = await response.Content.ReadFromJsonAsync<ApiErrorResponse>(cancellationToken);
		if ( errorResponse is not null )
		{
			_logger.LogError("{code}: {message}", errorResponse.Error.Code, errorResponse.Error.Message);
		}

		return null;
	}
}
