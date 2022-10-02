using MetaQuotes.App.Infrastructure.Filters;
using MetaQuotes.App.Infrastructure.HostedServices;
using MetaQuotes.Geobase.Contracts;

namespace MetaQuotes.App.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddControllers(options => options.Filters.Add<GlobalExceptionFilter>());
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddGeobase<GeobaseHostedService>();
        return services;
    }

    public static IServiceCollection AddGeobase<TService>(this IServiceCollection services)
        where TService : class, IHostedService, IGeobaseProvider
    {
        services.AddHostedService<TService>()
            .AddSingleton<IGeobaseProvider>(
                p => p.GetServices<IHostedService>().OfType<TService>().FirstOrDefault());
        return services;
    }
}