namespace AddressValidation.Abstractions;

using FluentValidation;
using FluentValidation.Results;
using Model;

public abstract class AddressValidationAbstractService<TRequest, TResponse> : IAddressValidationService<TRequest>
	where TRequest : AddressValidationAbstractRequest, new()
	where TResponse : IApiResponse, new()
{
	private readonly IValidator<TRequest> _requestValidator;

	private readonly IValidator<TResponse> _responseValidator;

	protected AddressValidationAbstractService(IValidator<TRequest> requestValidator,
											   IValidator<TResponse> responseValidator)
	{
		_requestValidator = requestValidator ?? throw new ArgumentNullException(nameof(requestValidator));
		_responseValidator = responseValidator ?? throw new ArgumentNullException(nameof(responseValidator));
	}

	public ValueTask<IAddressValidationResponse?> ValidateAsync(TRequest request, CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(request);
		return ValidateInternalAsync(request, cancellationToken);
	}

	protected abstract ValueTask<IAddressValidationResponse?> ConvertResponseAsync(TResponse response, ValidationResult? responseValidationResult, CancellationToken cancellationToken);

	protected abstract ValueTask<TResponse?> SendRequestAsync(TRequest request, CancellationToken cancellationToken);

	private async ValueTask<IAddressValidationResponse?> ValidateInternalAsync(TRequest request, CancellationToken cancellationToken)
	{
		ValidationResult? requestValidationResult = await _requestValidator.ValidateAsync(request, cancellationToken);
		if ( requestValidationResult is not null &&
			 !requestValidationResult.IsValid &&
			 requestValidationResult.Errors.Any(a => a.Severity == Severity.Error) )
		{
			return new EmptyAddressValidationResponse(requestValidationResult);
		}

		TResponse? response = await SendRequestAsync(request, cancellationToken);
		if ( response is null )
		{
			return null;
		}

		ValidationResult? responseValidationResult = await _responseValidator.ValidateAsync(response, cancellationToken);
		if ( responseValidationResult is not null &&
			 !responseValidationResult.IsValid &&
			 responseValidationResult.Errors.Any(a => a.Severity == Severity.Error) )
		{
			return new EmptyAddressValidationResponse(requestValidationResult);
		}

		return await ConvertResponseAsync(response, responseValidationResult, cancellationToken);
	}
}
