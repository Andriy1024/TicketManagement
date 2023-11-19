using Marten.Events.Daemon.Resiliency;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

using Marten;
using Marten.Events;
using Marten.Events.Daemon.Resiliency;
using Marten.Events.Projections;
using Marten.Services.Json;
using Weasel.Core;

using TMS.Common.Extensions;
using TMS.Common.Interfaces;
using TMS.Payments.Persistence.EventStore;
using TMS.Payments.Application.Interfaces;
using TMS.Payments.Domain.DomainEvents;

namespace TMS.Payments.Persistence;

public static class ServiceRegistration
{

    public static IServiceCollection AddMarten(this IServiceCollection app, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DbConnection");

        app.AddMarten(o =>
        {
            o.Connection(connectionString.ThrowIfNullOrEmpty());

            o.AutoCreateSchemaObjects = AutoCreate.All;
            o.DatabaseSchemaName = "DocumentStore";
            o.Events.DatabaseSchemaName = "EventStore";

            // This is enough to tell Marten that the ConversationView document is persisted and needs schema objects
            //o.Schema.For<ConversationView>()
            //    //.UseOptimisticConcurrency(true)
            //    .Identity(x => x.Id);

            //o.Schema.For<UserConversationsView>()
            //    .Identity(x => x.UserId);

            //o.Projections.Add<ConversationViewProjection>(ProjectionLifecycle.Async);
            //o.Projections.Add<UserConversationsViewProjection>(ProjectionLifecycle.Async);

            // Lets Marten know that the event store is active
            o.Events.AddEventType(typeof(PaymentCreatedEvent));
            o.Events.AddEventType(typeof(PaymentStatusUpdated));

            o.Events.StreamIdentity = StreamIdentity.AsString;

            // Opt into System.Text.Json serialization
            o.UseDefaultSerialization(
                serializerType: SerializerType.SystemTextJson,
                // Optionally override the enum storage
                enumStorage: EnumStorage.AsString,
                // Optionally override the member casing
                casing: Casing.CamelCase
            );
        })
       // Chained helper to replace the built in
       // session factory behavior
       .UseLightweightSessions()
       // Using the "Optimized artifact workflow" for Marten >= V5
       // sets up your Marten configuration based on your environment
       // See https://martendb.io/configuration/optimized_artifact_workflow.html
       .OptimizeArtifactWorkflow()
       // Optionally apply all database schema
       // changes on startup
       .ApplyAllDatabaseChangesOnStartup()
       .InitializeWith()
       // Turn on the async daemon in "Solo" mode
       .AddAsyncDaemon(DaemonMode.Solo);

        app.AddOptions<MartenConfig>()
            .BindConfiguration(nameof(MartenConfig))
            .Services
            .AddScoped<IStartupTask, DataBaseStartupTask>()
            .AddScoped<IPaymentsEventStore, MartenEventStore>();

        return app;
    }
}
