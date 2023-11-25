using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using System.Text;

namespace TMS.Test.Common;

public class ServicesBuilder<T> where T : ServicesBuilder<T>
{
    public Dictionary<string, string?> InMemoryConfig { get; set; }

    public ConfigurationBuilder ConfigBuilder { get; set; }

    public IConfiguration Config { get; set; }

    public ServiceCollection Services { get; set; }

    public ServicesBuilder()
    {
        InMemoryConfig = new();
        ConfigBuilder = new();
        Services = new();
    }

    public T AddJsonConfig(params string[] path)
    {
        ArgumentNullException.ThrowIfNull(path);

        var config = FileReader.ReadAsString(path);

        ConfigBuilder.AddJsonStream(new MemoryStream(Encoding.UTF8.GetBytes(config)));

        return (T)this;
    }

    public T AddConfigValue(string key, string value) 
    {
        ArgumentNullException.ThrowIfNullOrEmpty(key);
        ArgumentNullException.ThrowIfNullOrEmpty(value);

        InMemoryConfig.Add(key, value);

        return (T)this;
    }

    public T BuildConfiguration()
    {
        ConfigBuilder.AddInMemoryCollection(InMemoryConfig);

        Config = ConfigBuilder.Build();

        Services.AddSingleton<IConfiguration>(Config);

        return (T)this;
    }

    public T OverrideService<TService>(TService service) where TService : class 
    {
        Services.Remove<TService>();

        Services.AddSingleton(service);

        return (T)this;
    }

    public IServiceProvider BuildServices() => Services.BuildServiceProvider();

    public IServiceScope BuildServicesScope() => BuildServices().CreateScope();
}