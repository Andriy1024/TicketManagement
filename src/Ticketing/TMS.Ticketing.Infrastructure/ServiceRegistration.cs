using MediatR;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using System.Reflection;

using Refit;

using TMS.Common.Users;
using TMS.Common.Validation;

using TMS.Caching.Redis;

using TMS.Ticketing.Application.Interfaces;
using TMS.Ticketing.Infrastructure.ChangeTracker;
using TMS.Ticketing.Infrastructure.DomainEvents;
using TMS.Ticketing.Infrastructure.Payments;
using TMS.Ticketing.Infrastructure.Payments.API;
using TMS.Ticketing.Application.UseCases.Carts;
using TMS.Ticketing.Infrastructure.Transactions;
using TMS.RabbitMq.Configuration;
using TMS.Common.Interfaces;
using TMS.Ticketing.Infrastructure.MessageBroker;

namespace TMS.Ticketing.Infrastructure;

public static class ServiceRegistration
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var paymentsConfig = configuration.GetSection(nameof(PaymentsConfig)).Get<PaymentsConfig>()
            ?? throw new ArgumentNullException(nameof(PaymentsConfig));

        return services
            .AddMediatR(x => x.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly(), typeof(GetCartDetails).Assembly))
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>))
            .AddCachableBehavior()
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>))
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(DomainEventsDispatcher<,>))
            .AddScoped<IUserContext, UserContext>()
            .AddScoped<IEntityChangeTracker, EntityChangeTracker>()
            .AddTransient<IPaymentsService, PaymentsService>()
            .AddRefitClient<IPaymentsApi>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(paymentsConfig.PaymentsUri))
            .Services
            .AddRedisServices<DB1>(configuration)
            .AddRabbitMqMessageBus(configuration)
            .AddTransient<IStartupTask, RabbitMqStartupTask>();
    }
}

