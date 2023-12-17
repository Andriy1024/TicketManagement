using System.Diagnostics;
using System.Reflection;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

using OpenTelemetry.Exporter;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OpenTelemetry.Logs;

namespace TMS.Observability;

public static class OpenTelemetrySetup
{
    public static readonly string Name = "TMS";

    private static readonly ActivitySource Source = new ActivitySource(Name);

    public static WebApplicationBuilder AddOpenTelemetry(this WebApplicationBuilder app)
    {
        var config = app.Configuration.GetSection(nameof(JaegerConfig)).Get<JaegerConfig>()
            ?? throw new ArgumentNullException(nameof(JaegerConfig));

        Activity.DefaultIdFormat = ActivityIdFormat.W3C;

        string serviceVersion = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? string.Empty;

        var serviceName = $"{app.Environment.ApplicationName}.{app.Environment.EnvironmentName}";

        app.Services
            .AddOpenTelemetry()
            .WithTracing(tracing =>
            {
                tracing
                    .AddSource(Name)
                    .AddConsoleExporter(options =>
                    {
                        options.Targets = ConsoleExporterOutputTargets.Console;
                    })
                    .AddAspNetCoreInstrumentation(options =>
                    {
                        // Exclude swagger
                        options.Filter = c =>
                            !c.Request.Path.Value.Contains("swagger") &&
                            !c.Request.Path.Value.Contains("_vs/browserLink") &&
                            !c.Request.Path.Value.Contains("_framework/aspnetcore-browser-refresh.js");
                        options.RecordException = true;
                    })
                    .AddRedisInstrumentation()
                    .AddHttpClientInstrumentation(options =>
                    {
                        options.Filter = message =>
                            message != null &&
                            message.RequestUri != null &&
                            !message.RequestUri.Host.Contains("visualstudio");
                    })
                    .SetSampler(new AlwaysOnSampler());

                if (config.Enabled)
                {
                    tracing.AddJaegerExporter(o =>
                    {
                        o.AgentHost = config.JAEGER_HOST;
                        o.AgentPort = config.JAEGER_PORT;
                    });
                }
            })
            .ConfigureResource(builder =>    
            {
                builder
                .AddTelemetrySdk()
                .AddService(serviceName, serviceVersion: serviceVersion, serviceInstanceId: Environment.MachineName);
            });

        return app;
    }
}