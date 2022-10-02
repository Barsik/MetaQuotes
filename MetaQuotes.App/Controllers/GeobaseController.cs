using MetaQuotes.Geobase;
using MetaQuotes.Geobase.Contracts;
using MetaQuotes.Geobase.Models;
using Microsoft.AspNetCore.Mvc;

namespace MetaQuotes.App.Controllers;

[ApiController]
public class GeobaseController : ControllerBase
{
    private readonly IGeobaseProvider _geobaseProvider;
    private readonly ILogger<GeobaseController> _logger;

    public GeobaseController(IGeobaseProvider geobaseProvider, ILogger<GeobaseController> logger)
    {
        _geobaseProvider = geobaseProvider;
        _logger = logger;
    }
    
    
    [HttpGet("/ip/location")]
    public ActionResult<CoordinatesView?> GetCoordinatesByIp(string ip)
    {
        var result=_geobaseProvider.GetCoordinatesByIp(ip);
        if (result == null)
        {
            return NotFound();
        }

        return result;
    }
    
    [HttpGet("/city/locations")]
    public ActionResult<List<LocationView>> GetLocationsByCity(string city)
    {
        return _geobaseProvider.GetLocationsByCity(city);
    }
}