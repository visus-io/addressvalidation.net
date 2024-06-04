namespace AddressValidation.Google.Extensions;

using System.Diagnostics.CodeAnalysis;
using AddressValidation.Abstractions;
using FluentValidation;
using Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Polly;
using Polly.Extensions.Http;
using Services;
using Validation;

/// <summary>
///     Extension methods for setting up Google Address Validation services in an <see cref="IServiceCollection" />.
/// </summary>
public static class ServiceCollectionExtensions
{
	private const string LoggingCategoryName = "AddressValidation.Google";

	private const int TransientRetryCount = 3;

	/// <summary>
	///     Adds <see cref="IAddressValidationService{TRequest}" /> and related services to the
	///     <see cref="IServiceCollection" />.
	/// </summary>
	/// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
	/// <returns>The same service collection so that multiple calls can be chained.</returns>
	public static IServiceCollection AddGoogleAddressValidation(this IServiceCollection services)
	{
		ArgumentNullException.ThrowIfNull(services);

		services.TryAddScoped<IValidator<GoogleAddressValidationRequest>, AddressValidationRequestValidator>();
		services.TryAddScoped<IAddressValidationRequest, GoogleAddressValidationRequest>();
		services.TryAddScoped<IAddressValidationService<GoogleAddressValidationRequest>, AddressValidationService>();

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
