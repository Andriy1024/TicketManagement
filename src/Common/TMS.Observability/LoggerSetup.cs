using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Builder;

using Serilog;
using Serilog.Events;
using Serilog.Enrichers.Span;
using Serilog.Formatting.Compact;

namespace TMS.Observability;

public static class LoggerSetup
{
    public static IHostBuilder UseLogger(this IHostBuilder host)
    {
        host.UseSerilog((context, services, config) =>
        {
            config
                 .Enrich.WithProperty("Application", context.HostingEnvironment.ApplicationName)
                 .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName)
                 .MinimumLevel.Information()
                 .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                 .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Warning)
                 .Enrich.FromLogContext()
                 //.Enrich.WithExceptionDetails()
                 .Enrich.WithSpan()
                 .WriteTo.Async(wt => wt.Console())
                 .WriteTo.Async(wt => wt.File(
                    new CompactJsonFormatter(),
                    path: "logs/log.txt",
                    restrictedToMinimumLevel: LogEventLevel.Information,
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 30))
                 .ReadFrom.Configuration(context.Configuration);
        });

        return host;
    }

    public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder app)
    {
        return app.UseSerilogRequestLogging();
    }
}
