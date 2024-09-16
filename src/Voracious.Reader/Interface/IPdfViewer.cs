namespace Voracious.Reader.Interface;

public interface IPdfViewer
{
    /// <summary>
    /// The PDF document stream that is loaded into the instance of the PDF viewer. 
    /// </summary>
    public Stream PdfDocumentStream { get; set; }
}