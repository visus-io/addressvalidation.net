namespace AddressValidation.Google.Extensions;

using System.Diagnostics.CodeAnalysis;
using Abstractions;
using AddressValidation.Abstractions;
using AddressValidation.Http;
using Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Extensions.Http;
using Refit;

public static class ServiceCollectionExtensions
{
	private const string GoogleAddressValidationApi = "GoogleAddressValidationApi";

	private static readonly Uri GoogleAddressValidationBaseUri = new("https://addressvalidation.googleapis.com");

	private const int TransientRetryCount = 3;

	public static IServiceCollection AddGoogleAddressValidationHttpClient(this IServiceCollection services)
	{
		ArgumentNullException.ThrowIfNull(services);

		services.AddSingleton<ApiKeyDelegateHandler>();
		
		services.AddTransient<IAddressValidationRequest, AddressValidationRequest>();

		services.AddHttpClient(GoogleAddressValidationApi,
							   client => client.BaseAddress = GoogleAddressValidationBaseUri);

		services.AddRefitClient<IAddressValidationClient>()
				.ConfigureHttpClient(client => client.BaseAddress = GoogleAddressValidationBaseUri)
				.AddPolicyHandler((provider, _) => GetHttpRetryPolicy(provider.GetRequiredService<ILogger>()))
				.AddHttpMessageHandler<ApiKeyDelegateHandler>();

		return services;
	}

	[ExcludeFromCodeCoverage]
	private static AsyncPolicy<HttpResponseMessage> GetHttpRetryPolicy(ILogger logger)
	{
		return HttpPolicyExtensions.HandleTransientHttpError()
								   .WaitAndRetryAsync(TransientRetryCount,
													  retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
													  (exception, _, retries, context) =>
													  {
														  if ( TransientRetryCount != retries )
														  {
															  return;
														  }

														  string message = $"#Polly #WaitAndRetryAsync Retry {retries}" +
																		   $"of {context.PolicyKey}" +
																		   $"due to: {exception}";

														  logger.LogError("{message}", message);
													  });
	}
}
