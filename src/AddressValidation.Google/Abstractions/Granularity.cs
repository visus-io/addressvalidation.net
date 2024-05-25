namespace AddressValidation.Google.Abstractions;

using System.ComponentModel;
using System.Runtime.Serialization;

internal enum Granularity
{
	[Description("Default value. This value is unused.")]
	[EnumMember(Value = "GRANULARITY_UNSPECIFIED")]
	GranularityUnspecified = 0,
	
	[Description("Below-building level result, such as an apartment.")]
	[EnumMember(Value = "SUB_PREMISE")]
	SubPremise = 1,
	
	[Description("Building-level result.")]
	[EnumMember(Value = "PREMISE")]
	Premise = 2,
	
	[Description("A geocode that approximates the building-level location of the address.")]
	[EnumMember(Value = "PREMISE_PROXIMITY")]
	PremiseProximity = 3,
	
	[Description("The address or geocode indicates a block. Only used in regions which have block-level addressing, such as Japan.")]
	[EnumMember(Value = "BLOCK")]
	Block = 4,
	
	[Description("The geocode or address is granular to route, such as a street, road, or highway.")]
	[EnumMember(Value = "ROUTE")]
	Route = 5,
	
	[Description("All other granularities, which are bucketed together since they are not deliverable.")]
	[EnumMember(Value = "OTHER")]
	Other = 6
}
