namespace Voracious.EPub;

public class EpubParseException : EpubException
{
    public EpubParseException(string message) : base($"EPUB parsing error: {message}") { }
}
