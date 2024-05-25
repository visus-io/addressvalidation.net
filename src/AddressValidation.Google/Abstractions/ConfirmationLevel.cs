namespace AddressValidation.Google.Abstractions;

using System.ComponentModel;
using System.Runtime.Serialization;

internal enum ConfirmationLevel
{
	[Description("Default value. This value is unused.")]
	[EnumMember(Value = "CONFIRMATION_LEVEL_UNSPECIFIED")]
	ConfirmationLevelUnspecified = 0,
	
	[Description("We were able to verify that this component exists and makes sense in the context of the rest of the address.")]
	[EnumMember(Value = "CONFIRMED")]
	Confirmed = 1,
	
	[Description("This component could not be confirmed, but it is plausible that it exists. For example, a street number within a known valid range of numbers on a street where specific house numbers are not known.")]
	[EnumMember(Value = "UNCONFIRMED_BUT_PLAUSIBLE")]
	UnconfirmedButPlausible = 2,

	[Description("This component was not confirmed and is likely to be wrong. For example, a neighborhood that does not fit the rest of the address.")]
	[EnumMember(Value = "UNCONFIRMED_AND_SUSPICIOUS")]
	UnconfirmedAndSuspicious = 3
}
