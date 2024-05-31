namespace AddressValidation.Tests;

using Abstractions;
using FluentValidation.TestHelper;

public sealed class AddressValidationRequestAbstractValidatorFacts
{
	[Fact]
	public void AddressValidationRequestValidator_Success()
	{
		// Google US
		var request = new TestAddressValidationRequest
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

		var validator = new TestAddressValidationRequestValidator();
		var result = validator.TestValidate(request);

		result.ShouldNotHaveAnyValidationErrors();
	}

	[Fact]
	public void AddressValidationRequestValidator_Unsupported_Country()
	{
		// U.S Embassy in Zimbabwe
		var request = new TestAddressValidationRequest
		{
			AddressLines =
			{
				"2 Lorraine Dr"
			},
			CityOrTown = "Harare",
			Country = CountryCode.ZW
		};

		var validator = new TestAddressValidationRequestValidator();
		var result = validator.TestValidate(request);

		result.ShouldHaveValidationErrorFor(f => f.Country);
	}

	private class TestAddressValidationRequest : AbstractAddressValidationRequest
	{
	}

	private class TestAddressValidationRequestValidator : AbstractAddressValidationRequestValidator<TestAddressValidationRequest>
	{
	}
}
