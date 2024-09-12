using CommunityToolkit.Mvvm.ComponentModel;
using Voracious.Reader.Interface;

namespace Voracious.Reader.ViewModel;

public partial class PdfViewerViewModel : ObservableObject, IPdfViewer
{
    /// <summary>
    /// The PDF document stream that is loaded into the instance of the PDF viewer. 
    /// </summary>
    [ObservableProperty]
    private Stream pdfDocumentStream;

    /// <summary>
    /// Constructor of the view model class
    /// </summary>
    public PdfViewerViewModel()
    {
    }
}
