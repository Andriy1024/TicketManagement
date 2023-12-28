﻿using System.Reflection;

namespace TMS.RabbitMq.Configuration;

public class RabbitMqConfiguration
{
    /// <summary>
    /// The name of the connecting server. 
    /// For example: IdentityServer, Realtime.
    /// </summary>
    public string ConnectionName { get; set; }

    public ResilienceConfiguration Resilience { get; set; }

    public RabbitMqConnectionConfiguration Connection { get; set; }

    public static RabbitMqConfiguration Validate(RabbitMqConfiguration options)
    {
        options ??= new RabbitMqConfiguration()
        {
            ConnectionName = Assembly.GetEntryAssembly().GetName().Name
        };

        options.Connection ??= new RabbitMqConnectionConfiguration();
        options.Resilience ??= new ResilienceConfiguration();

        return options;
    }
}
