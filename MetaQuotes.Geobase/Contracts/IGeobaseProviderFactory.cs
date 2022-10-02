namespace MetaQuotes.Geobase.Contracts;

/// <summary>
/// Интерфейс фэктори для создания IGeobaseProvider
/// </summary>
public interface IGeobaseProviderFactory
{
    IGeobaseProvider Create();
}