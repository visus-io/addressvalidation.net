namespace AddressValidation.Google.Tests;

using AddressValidation.Abstractions;
using FluentValidation.TestHelper;
using Http;
using Validation;

public sealed class AddressValidationRequestValidatorFacts
{
	[Fact]
	public void AddressValidationRequestValidator_Success()
	{
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

		var validator = new AddressValidationRequestValidator();
		var result = validator.TestValidate(request);

		result.ShouldNotHaveAnyValidationErrors();
	}

	[Fact]
	public void AddressValidationRequestValidator_Unsupported_Region()
	{
		// U.S. Embassy (Belarus)
		var request = new AddressValidationRequest
		{
			AddressLines =
			{
				"Ulitsa Starovilenskaya 46"
			},
			CityOrTown = "Minsk",
			StateOrProvince = "Minskaja voblasć",
			Country = CountryCode.BY
		};
		
		var validator = new AddressValidationRequestValidator();
		var result = validator.TestValidate(request);

		result.ShouldHaveValidationErrorFor(f => f.Country);
	}
}
