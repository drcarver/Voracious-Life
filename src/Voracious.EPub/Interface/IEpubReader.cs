using System.IO;
using System.Text;

namespace Voracious.EPub.Interface;

public interface IEpubReader
{
    /// <summary>
    /// Read a EPUB from a file
    /// </summary>
    /// <param name="filePath">The file path to open</param>
    /// <param name="encoding">The encoding of the file</param>
    /// <returns>The EpubBook we have just read in</returns>
    public EpubBook Read(string filePath, Encoding encoding = null);

    /// <summary>
    /// Read a EPUB from a byte array
    /// </summary>
    /// <param name="epubData">The epub as a byte array</param>
    /// <param name="encoding">The encoding of the file</param>
    /// <returns>The EpubBook we have just read in</returns>
    public EpubBook Read(byte[] epubData, Encoding encoding = null);

    /// <summary>
    /// Read a EPUB from a stream
    /// </summary>
    /// <param name="stream">The epub as a stream</param>
    /// <param name="leaveOpen">Leave the stream open</param>
    /// <param name="encoding">The encoding of the file</param>
    /// <returns>The EpubBook we have just read in</returns>
    public EpubBook Read(Stream stream, bool leaveOpen, Encoding encoding = null);
}