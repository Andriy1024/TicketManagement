using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using TMS.Common.Caching;
using TMS.Ticketing.Application.Interfaces;
using TMS.Ticketing.Persistence;

namespace TMS.Ticketing.IntegrationTest.Common;

public class TicketingApiFactory : WebApplicationFactory<Program>
{
    private readonly Dictionary<string, string?> InMemoryConfig = new();

    public TicketingApiFactory(MongoConfig mongo)
    {
        AddConfigValue(
            $"{nameof(MongoConfig)}:{nameof(MongoConfig.DatabaseName)}",
            mongo.DatabaseName);

        AddConfigValue(
            $"{nameof(MongoConfig)}:{nameof(MongoConfig.ConnectionString)}",
            mongo.ConnectionString);
    }

    public TicketingApiFactory AddConfigValue(string key, string value)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(key);
        ArgumentNullException.ThrowIfNullOrEmpty(value);

        InMemoryConfig.Add(key, value);

        return this;
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureHostConfiguration(config =>
        {
            config.AddInMemoryCollection(InMemoryConfig);
        });

        return base.CreateHost(builder);
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services => 
        {
            services.Remove<IPaymentsService>();

            services.AddSingleton(Substitute.For<IPaymentsService>());

            services.AddSingleton<ICacheClient>(new FakeCacheClient());
        });
    }

    public HttpClient CreateApiClient()
    {
        return CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }
}