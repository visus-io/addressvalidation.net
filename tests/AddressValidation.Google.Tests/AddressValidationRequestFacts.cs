namespace AddressValidation.Google.Tests;

using System.Text.Json;
using AddressValidation.Abstractions;
using Http;
using Newtonsoft.Json.Linq;

public class AddressValidationRequestFacts
{
	[Fact]
	public void Serialization_CityState_Success()
	{
		const string expected = """{"address":{"addressLines":["1 Lim Ah Pin Road"],"postalCode":"547809","regionCode":"SG"},"enableUspsCass":false}""";

		// Singapore Post (North East)
		var request = new AddressValidationRequest
		{
			AddressLines =
			{
				"1 Lim Ah Pin Road"
			},
			PostalCode = "547809",
			Country = CountryCode.SG
		};

		var result = JsonSerializer.Serialize(request);

		Assert.True(JToken.DeepEquals(JToken.Parse(expected), JToken.Parse(result)));
	}

	[Fact]
	public void Serialization_Default_Success()
	{
		const string expected = """{"address":{"addressLines":["1600 Amphitheatre Pkwy"],"administrativeArea":"CA","locality":"Mountain View","postalCode":"94043","regionCode":"US"},"enableUspsCass":true}""";

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

		var result = JsonSerializer.Serialize(request);

		Assert.True(JToken.DeepEquals(JToken.Parse(expected), JToken.Parse(result)));
	}

	[Fact]
	public void Serialization_NoUspsCass_Success()
	{
		const string expected = """{"address":{"addressLines":["111 Richmond Street West"],"administrativeArea":"ON","locality":"Toronto","postalCode":"M5H 2G4","regionCode":"CA"},"enableUspsCass":false}""";

		// Google Canada
		var request = new AddressValidationRequest
		{
			AddressLines =
			{
				"111 Richmond Street West"
			},
			CityOrTown = "Toronto",
			StateOrProvince = "ON",
			PostalCode = "M5H 2G4",
			Country = CountryCode.CA
		};

		var result = JsonSerializer.Serialize(request);

		Assert.True(JToken.DeepEquals(JToken.Parse(expected), JToken.Parse(result)));
	}
}
