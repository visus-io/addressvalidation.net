namespace AddressValidation.Google.Http;

using System.Net;

internal sealed class ApiErrorResponse
{
	public HttpStatusCode Code { get; set; }

	public string? Message { get; set; }
}
