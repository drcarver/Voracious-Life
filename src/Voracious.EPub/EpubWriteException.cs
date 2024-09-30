namespace Voracious.EPub;

public class EpubWriteException : EpubException
{
    public EpubWriteException(string message) : base($"EPUB write error: {message}") { }
}
