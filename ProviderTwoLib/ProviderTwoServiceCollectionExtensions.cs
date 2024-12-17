using SearchApiShared;

namespace ProviderTwoLib;

public static class ProviderTwoServiceCollectionExtensions
{    
    public static IServiceCollection AddProviderTwo(this IServiceCollection services, string baseAddress)
    {
        services.AddHttpClient(
            nameof(ProviderTwoSearchProvider),
            client =>
            {
                client.BaseAddress = new Uri(new Uri(baseAddress), "api/v1/");
            });

        return services
            .AddSingleton<ProviderTwoLibMappings>()            
            .AddTransient<ISearchProvider, ProviderTwoSearchProvider>();
    }
}
