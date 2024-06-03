namespace AddressValidation.Google.Extensions;

using System.Diagnostics.CodeAnalysis;
using AddressValidation.Abstractions;
using FluentValidation;
using Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Polly;
using Polly.Extensions.Http;
using Validation;

public static class ServiceCollectionExtensions
{
	private const string LoggingCategoryName = "AddressValidation.Google";

	private const int TransientRetryCount = 3;

	public static IServiceCollection AddGoogleAddressValidationHttpClient(this IServiceCollection services)
	{
		ArgumentNullException.ThrowIfNull(services);

		services.AddScoped<IValidator<AddressValidationRequest>, AddressValidationRequestValidator>();

		services.AddSingleton<ApiKeyDelegateHandler>();

		services.AddTransient<IAddressValidationRequest, AddressValidationRequest>();

		services.AddHttpClient<AddressValidationClient>()
				.AddPolicyHandler((provider, _) => GetHttpRetryPolicy(GetLoggerInstance(provider)))
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

	[ExcludeFromCodeCoverage]
	private static ILogger GetLoggerInstance(IServiceProvider provider)
	{
		ILoggerFactory? factory = provider.GetService<ILoggerFactory>();
		return factory is null
				   ? NullLoggerFactory.Instance.CreateLogger(LoggingCategoryName)
				   : factory.CreateLogger(LoggingCategoryName);
	}
}
