using Microsoft.Extensions.DependencyInjection;
using Microsoft.Kiota.Http.HttpClientLibrary;
using SearchApiShared;

namespace ProviderOneLib;

public static class ProviderOneServiceCollectionExtensions
{    
    public static IServiceCollection AddKiotaHandlers(this IServiceCollection services)
    {        
        var kiotaHandlers = KiotaClientFactory.GetDefaultHandlerTypes();     
        foreach (var handler in kiotaHandlers)
        {
            services.AddTransient(handler);
        }

        return services;
    }

    public static IHttpClientBuilder AttachKiotaHandlers(this IHttpClientBuilder builder)
    {        
        var kiotaHandlers = KiotaClientFactory.GetDefaultHandlerTypes();     
        foreach (var handler in kiotaHandlers)
        {
            builder.AddHttpMessageHandler((sp) => (DelegatingHandler)sp.GetRequiredService(handler));
        }

        return builder;
    }

    public static IServiceCollection AddProviderOne(this IServiceCollection services, string baseAddress)
    {        
        services.AddKiotaHandlers();
        
        services.AddHttpClient<ProviderOneClientFactory>((sp, client) => {
            client.BaseAddress = new Uri(baseAddress);            
        }).AttachKiotaHandlers();

        return services
            .AddSingleton<ProviderOneLibMappings>()
            .AddTransient(sp => sp.GetRequiredService<ProviderOneClientFactory>().GetClient())
            .AddTransient<ISearchProvider, ProviderOneSearchProvider>();
    }
}
