namespace AddressValidation.Tests;

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Abstractions;
using PublicApiGenerator;

[SuppressMessage("Performance", "CA1822:Mark members as static")]
public sealed class ApiFacts
{
	[Fact]
	[MethodImpl(MethodImplOptions.NoInlining)]
	public async Task AddressValidation_NoBreakingChanges_Async()
	{
		var api = typeof(IAddressValidationRequest).Assembly.GeneratePublicApi(new ApiGeneratorOptions()
		{
			ExcludeAttributes = ["System.Runtime.Versioning.TargetFrameworkAttribute", "System.Reflection.AssemblyMetadataAttribute"]
		});

		await Verify(api);
	}
}
