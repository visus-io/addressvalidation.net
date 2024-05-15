namespace AddressValidation;

using Abstractions;

internal static class AddressGlobals
{
	public static IReadOnlySet<CountryCode> CityStates => new HashSet<CountryCode>
	{
		CountryCode.MC,
		CountryCode.SG,
		CountryCode.VA
	};
}
