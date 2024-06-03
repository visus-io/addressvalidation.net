namespace AddressValidation.Google.Tests;

using System.Web;
using Http;
using Microsoft.Extensions.Configuration;
using Moq;

public sealed class ApiKeyDelegateHandlerFacts
{
	[Fact]
	public async Task SendAsync_Default()
	{
		const string googleApiKey = "AVE_GOOGLE_API_KEY";
		const string apiKey = "nF4XVxKf3ncwp_8bM2MWWKfNJho-al37bdIVoMs2VJZzjh00QIiz7A";

		var configurationSectionMock = new Mock<IConfigurationSection>();
		configurationSectionMock.Setup(s => s.Value)
								.Returns(apiKey);

		var configurationMock = new Mock<IConfiguration>();
		configurationMock.Setup(s => s.GetSection(googleApiKey))
						 .Returns(configurationSectionMock.Object);

		var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, "http://localhost");
		var handler = new ApiKeyDelegateHandler(configurationMock.Object)
		{
			InnerHandler = new TestHandler()
		};

		var invoker = new HttpMessageInvoker(handler);
		_ = await invoker.SendAsync(httpRequestMessage, default);

		var query = HttpUtility.ParseQueryString(httpRequestMessage.RequestUri!.Query);

		Assert.NotNull(query["key"]);
		Assert.Equal(apiKey, query["key"]);
	}

	[Fact]
	public async Task SendAsync_ThrowsInvalidOperationException()
	{
		const string googleApiKey = "AVE_GOOGLE_API_KEY";

		var configurationSectionMock = new Mock<IConfigurationSection>();
		configurationSectionMock.Setup(s => s.Value)
								.Returns(string.Empty);

		var configurationMock = new Mock<IConfiguration>();
		configurationMock.Setup(s => s.GetSection(googleApiKey))
						 .Returns(configurationSectionMock.Object);

		var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, "http://localhost");
		var handler = new ApiKeyDelegateHandler(configurationMock.Object)
		{
			InnerHandler = new TestHandler()
		};

		var invoker = new HttpMessageInvoker(handler);

		await Assert.ThrowsAsync<InvalidOperationException>(async () => await invoker.SendAsync(httpRequestMessage, default));
	}

	private sealed class TestHandler : DelegatingHandler
	{
		protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			return Task.FromResult(new HttpResponseMessage());
		}
	}
}
