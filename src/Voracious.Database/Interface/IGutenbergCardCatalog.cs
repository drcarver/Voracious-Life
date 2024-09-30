namespace Voracious.Database.Interface;

public interface ICardCatalog
{
    /// <summary>
    /// Read through the Gutenberg catalog and update new issues or
    /// add new entries.
    /// </summary>
    Task<int> UpdateCatalogAsync();
}