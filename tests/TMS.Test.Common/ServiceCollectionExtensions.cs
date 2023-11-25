using Microsoft.Extensions.DependencyInjection;

namespace TMS.Test.Common;

public static class ServiceCollectionExtensions 
{
    public static IServiceCollection Remove<T>(this IServiceCollection services) 
    {
        var serviceDescriptor = services.FirstOrDefault(
            x => x.ServiceType == typeof(T));

        if (serviceDescriptor != null)
        {
            services.Remove(serviceDescriptor);
        }

        return services;
    }
}