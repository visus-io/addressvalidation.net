namespace AddressValidation.Google.Tests;

using System.Net;
using System.Text.Json;
using Abstractions;
using AddressValidation.Abstractions;
using Http;
using Moq;
using Refit;
using Services;
using Validation;

public sealed class AddressValidationServiceFacts
{
	[Fact]
	public async Task Validation_Default_Success()
	{
		var json = await File.ReadAllTextAsync(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Fixtures", "DefaultResponse.json"));

		var clientMock = new Mock<IAddressValidationClient>();

		var requestValidator = new AddressValidationRequestValidator();
		var responseValidator = new ApiAddressValidationResponseValidator();
		var service = new AddressValidationService(clientMock.Object, requestValidator, responseValidator);

		// Google US
		var request = new AddressValidationRequest
		{
			AddressLines =
			{
				"1600 Amphitheatre Pkwy"
			},
			CityOrTown = "Mountain View",
			StateOrProvince = "CA",
			PostalCode = "94043",
			Country = CountryCode.US
		};

		clientMock.Setup(s => s.ValidateAddressAsync(request,
													 It.IsAny<CancellationToken>()))
				  .ReturnsAsync(() => new ApiResponse<ApiAddressValidationResponse>(new HttpResponseMessage
				   {
					   StatusCode = HttpStatusCode.OK
				   }, JsonSerializer.Deserialize<ApiAddressValidationResponse>(json), null!));

		var response = await service.ValidateAsync(request);

		Assert.NotNull(response);

		Assert.Equal(request.AddressLines.OrderBy(o => o), response.AddressLines.OrderBy(o => o));
		Assert.Equal(request.CityOrTown, response.CityOrTown);
		Assert.Equal(request.StateOrProvince, response.StateOrProvince);
		Assert.Equal(request.PostalCode, request.PostalCode);
		Assert.Equal(request.Country, response.Country);
	}
}
