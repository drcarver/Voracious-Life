using Voracious.Core.Model;

namespace Voracious.Core.Interface;

/// <summary>
/// One Gutenberg record for a book (not all data is saved)
/// </summary>
public interface ILibraryResourceCore
{
    /// <summary>
    /// The resource in the library
    /// </summary>
    public ResourceCore Resource { get; set; }
}
