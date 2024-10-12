using System;
using System.Collections.Generic;

using Voracious.Core.Interface;

namespace Voracious.Core.Model;

/// <summary>
/// One Gutenberg record for a book (not all data is saved)
/// </summary>
public partial class LibraryResource : ILibraryResourceCore
{
    /// <summary>
    /// The resource in the library
    /// </summary>
    public ResourceCore Resource { get; set; }
}
