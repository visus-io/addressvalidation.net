namespace AddressValidation;

using Abstractions;
using FluentValidation;
using FluentValidation.Results;
using Http.Abstractions;
using Model;

public abstract class AbstractAddressValidationService<TRequest, TResponse> : IAddressValidationService<TRequest>
	where TRequest : AbstractAddressValidationRequest
	where TResponse : IApiAddressValidationResponse
{
	private readonly IValidator<TRequest> _requestValidator;

	private readonly IValidator<TResponse> _responseValidator;

	protected AbstractAddressValidationService(IValidator<TRequest> requestValidator,
											   IValidator<TResponse> responseValidator)
	{
		if ( !requestValidator.GetType().IsSubclassOf(typeof(AbstractAddressValidationRequestValidator<TRequest>)) )
		{
			throw new Exception(); // TODO: Better exception
		}

		_requestValidator = requestValidator ?? throw new ArgumentNullException(nameof(requestValidator));
		_responseValidator = responseValidator ?? throw new ArgumentNullException(nameof(responseValidator));
	}

	public ValueTask<IAddressValidationResponse?> ValidateAsync(TRequest request, CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(request);
		return ValidateInternalAsync(request, cancellationToken);
	}

	protected abstract ValueTask<TResponse?> SendAsync(TRequest request, CancellationToken cancellationToken);

	private async ValueTask<IAddressValidationResponse?> ValidateInternalAsync(TRequest request, CancellationToken cancellationToken)
	{
		ValidationResult? requestValidationResult = await _requestValidator.ValidateAsync(request, cancellationToken);
		if ( requestValidationResult is not null &&
			 !requestValidationResult.IsValid &&
			 requestValidationResult.Errors.Any(a => a.Severity == Severity.Error) )
		{
			return new EmptyAddressValidationResponse(requestValidationResult);
		}

		TResponse? response = await SendAsync(request, cancellationToken);
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

		return response.ToAddressValidationResponse(responseValidationResult);
	}
}
