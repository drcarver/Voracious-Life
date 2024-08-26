using Voracious.EPub;

namespace Voracious.EpubSharp;

public class EpubWriteException : EpubException
{
    public EpubWriteException(string message) : base($"EPUB write error: {message}") { }
}
