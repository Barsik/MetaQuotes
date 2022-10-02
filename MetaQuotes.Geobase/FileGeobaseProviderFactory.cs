using MetaQuotes.Geobase.Contracts;

namespace MetaQuotes.Geobase;

/// <summary>
/// Фэктори для Geobase из файла
/// </summary>
public class FileGeobaseProviderFactory : IGeobaseProviderFactory
{
    private readonly string _geobaseFilePath;

    public FileGeobaseProviderFactory(string geobaseFilePath)
    {
        _geobaseFilePath = geobaseFilePath;
    }

    public IGeobaseProvider Create()
    {
        var data = File.ReadAllBytes(_geobaseFilePath);
        return GeobaseProvider.Load(data);
    }
}