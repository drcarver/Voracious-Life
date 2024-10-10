namespace Voracious.Control.Interface;

public interface IPdfViewer
{
    /// <summary>
    /// The PDF document stream that is loaded into the 
    /// instance of the PDF viewer. 
    /// </summary>
    public Stream PdfDocumentStream { get; set; }

    /// <summary>
    /// Are the flatten options visible
    /// </summary>
    bool FlattenOptionsVisible { get; set; }
}