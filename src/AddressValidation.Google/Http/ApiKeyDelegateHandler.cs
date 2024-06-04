namespace AddressValidation.Google.Http;

using System.Collections.Specialized;
using System.Web;
using Microsoft.Extensions.Configuration;

internal sealed class ApiKeyDelegateHandler(IConfiguration configuration) : DelegatingHandler
{
	private const string GoogleApiKey = "AVE_GOOGLE_API_KEY";

	private readonly IConfiguration _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

	protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		if ( request.RequestUri is null )
		{
			return base.SendAsync(request, cancellationToken);
		}

		string? apiKey = _configuration.GetValue<string>(GoogleApiKey);
		if ( string.IsNullOrWhiteSpace(apiKey) )
		{
			throw new InvalidOperationException();
		}

		UriBuilder uriBuilder = new(request.RequestUri);
		NameValueCollection query = HttpUtility.ParseQueryString(uriBuilder.Query);

		query.Add("key", apiKey);
		uriBuilder.Query = query.ToString();

		request.RequestUri = new Uri(uriBuilder.ToString());

		return base.SendAsync(request, cancellationToken);
	}
}
