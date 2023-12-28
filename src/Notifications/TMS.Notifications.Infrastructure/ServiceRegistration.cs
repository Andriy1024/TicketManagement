using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using TMS.Common.Interfaces;

using TMS.Notifications.Application.IntegrationEvents;
using TMS.Notifications.Application.Interfaces;
using TMS.Notifications.Infrastructure.BackgroundServices;
using TMS.Notifications.Infrastructure.Emails;
using TMS.Notifications.Infrastructure.MessageBrocker;

using TMS.RabbitMq.Configuration;

namespace TMS.Notifications.Infrastructure;

public static class ServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        return services
            .AddEmailServices(configuration)
            .AddRabbitMqMessageBus(configuration)
            .AddTransient<IStartupTask, RabbitMqStartupTask>()
            .AddMediatR(x => x.RegisterServicesFromAssemblyContaining<OrderStatusUpdatedNotificationHandler>())
            .AddHostedService<NotificationsBackgroundService>()
            ;
    }

    public static IServiceCollection AddEmailServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var config = configuration.GetSection(nameof(EmailConfig)).Get<EmailConfig>()
           ?? throw new ArgumentNullException(nameof(EmailConfig));

        if (!config.Enabled)
        {
            return services.AddTransient<IEmailsService, DisabledEmailsService>();
        }

        services.AddTransient<IEmailsService, EmailsService>();

        var fluendEmail = services
            .AddFluentEmail(config.FromEmail, config.FromName)
            .AddRazorRenderer();

        if (config.Provider == "SendGrid")
        {
            fluendEmail = fluendEmail.AddSendGridSender(config.SendGrid!.API_KEY);
        }
        else if (config.Provider == "SMPT")
        {
            fluendEmail = fluendEmail.AddSmtpSender(
                host: config.Smpt!.Host,
                port: config.Smpt.Port,
                username: config.Smpt.User,
                password: config.Smpt.Password);
        }
        else
        {
            throw new InvalidOperationException($"Unknown email provider: {config.Provider}");
        }

        return services;
    }
}