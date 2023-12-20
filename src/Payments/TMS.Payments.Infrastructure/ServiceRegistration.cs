using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using MediatR;
using Refit;

using TMS.Common.Users;
using TMS.Common.Validation;

using TMS.Payments.Application.UseCases;
using TMS.Payments.Infrastructure.MessageBrocker;
using TMS.Payments.Application.Interfaces;
using TMS.Payments.Infrastructure.Ticketing;
using TMS.RabbitMq.Configuration;

namespace TMS.Payments.Infrastructure;

public static class ServiceRegistration
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var ticketingConfig = configuration.GetSection(nameof(TicketingConfig)).Get<TicketingConfig>()
            ?? throw new ArgumentNullException(nameof(TicketingConfig));

        return services
            .AddScoped<IUserContext, UserContext>()
            .AddMediatR(x => x.RegisterServicesFromAssemblyContaining<CreatePaymentCommand>())
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>))
            .AddTransient<IPaymentsMessageBrocker, PaymentsMessageBrocker>()
            .AddRabbitMqMessageBus(configuration)
            .AddRefitClient<ITicketingApi>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(ticketingConfig.TicketingUri))
            .Services;
    }
}