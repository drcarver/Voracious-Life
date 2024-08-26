using System;

namespace Voracious.EPub;

public class EpubException : Exception
{
    public EpubException(string message) : base(message) { }
}
