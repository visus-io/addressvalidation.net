namespace AddressValidation.Tests;

using System.Runtime.CompilerServices;
using Abstractions;
using PublicApiGenerator;

public sealed class ApiFacts
{
	[Fact]
	[MethodImpl(MethodImplOptions.NoInlining)]
	public async Task AddressValidation_NoBreakingChanges_Async()
	{
		var api = typeof(IAddressValidationResponse).Assembly.GeneratePublicApi(new ApiGeneratorOptions
		{
			ExcludeAttributes = ["System.Runtime.Versioning.TargetFrameworkAttribute", "System.Reflection.AssemblyMetadataAttribute"]
		});

		await Verify(api);
	}
}
