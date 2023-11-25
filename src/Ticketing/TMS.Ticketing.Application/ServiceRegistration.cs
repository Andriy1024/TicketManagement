﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Refit;

using TMS.Common.Users;
using TMS.Ticketing.Application.Services.Payments;
using TMS.Ticketing.Application.UseCases.Carts;

namespace TMS.Ticketing.Application;

public static class ServiceRegistration
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        IConfiguration configuration) 
    {
        var paymentsConfig = configuration.GetSection(nameof(PaymentsConfig)).Get<PaymentsConfig>()
            ?? throw new ArgumentNullException(nameof(PaymentsConfig));

        services
            .AddScoped<IUserContext, UserContext>()
            .AddMediatR(x => x.RegisterServicesFromAssemblyContaining<GetCartDetails>())
            .AddTransient<IPaymentsService, PaymentsService>()
            .AddRefitClient<IPaymentsApi>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(paymentsConfig.PaymentsUri));

        return services;
    }
}