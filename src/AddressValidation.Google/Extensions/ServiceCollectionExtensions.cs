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
using Services;
using Validation;

public static class ServiceCollectionExtensions
{
	private const string LoggingCategoryName = "AddressValidation.Google";

	private const int TransientRetryCount = 3;

	public static IServiceCollection AddGoogleAddressValidation(this IServiceCollection services)
	{
		ArgumentNullException.ThrowIfNull(services);

		services.AddScoped<IValidator<GoogleAddressValidationRequest>, AddressValidationRequestValidator>();
		services.AddScoped<IAddressValidationRequest, GoogleAddressValidationRequest>();
		services.AddScoped<IAddressValidationService<GoogleAddressValidationRequest>, AddressValidationService>();

		services.AddTransient<ApiKeyDelegateHandler>();

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
													  retryAttempt => TimeSpan.FromSeconds(Math.Pow(5, retryAttempt)),
													  (exception, timeSpan, retries, context) =>
													  {
														  if ( retries < TransientRetryCount )
														  {
															  logger.LogWarning("Retry {retry} for {policy} delayed by {delay}ms.",
																				retries,
																				context.PolicyKey,
																				timeSpan.TotalMilliseconds);
														  }

														  if ( TransientRetryCount != retries )
														  {
															  return;
														  }
														  
														  logger.LogError("Retry {retries} for {policy} failed due to {exception}.", 
																		  retries, 
																		  context.PolicyKey, 
																		  exception);
													  })
								   .WithPolicyKey($"{LoggingCategoryName}.HttpRetryPolicy");
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
