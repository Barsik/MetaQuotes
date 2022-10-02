using MetaQuotes.Geobase;
using MetaQuotes.Geobase.Contracts;
using MetaQuotes.Geobase.Models;

namespace MetaQuotes.App.Infrastructure.HostedServices;

/// <summary>
/// Прокси сервис для IGeobaseProvider
/// </summary>
public class GeobaseHostedService : IHostedService, IGeobaseProvider
{
    private readonly IConfiguration _configuration;
    private IGeobaseProvider _geobaseProvider;

    public GeobaseHostedService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        var factory = new FileGeobaseProviderFactory(_configuration["geobase:path"]);
        _geobaseProvider = factory.Create();
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public CoordinatesView? GetCoordinatesByIp(string ipStr)
    {
        return _geobaseProvider.GetCoordinatesByIp(ipStr);
    }

    public List<LocationView> GetLocationsByCity(string city)
    {
        return _geobaseProvider.GetLocationsByCity(city);
    }
    
}