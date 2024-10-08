using System.Threading;

using CommunityToolkit.Mvvm.ComponentModel;

using Microsoft.Extensions.Logging;

using Voracious.Life.Interface;

namespace Voracious.Life.ViewModel;

public partial class PdfViewerViewModel : ObservableObject, IPdfViewer
{
    /// <summary>
    /// The PDF document stream that is loaded into the instance of the PDF viewer. 
    /// </summary>
    [ObservableProperty]
    private Stream pdfDocumentStream;

    /// <summary>
    /// Are the flatten options visible
    /// </summary>
    [ObservableProperty]
    private bool flattenOptionsVisible;

    /// <summary>
    /// The logger property
    /// </summary>
    private ILogger logger { get; }

    /// <summary>
    /// Constructor of the view model class
    /// </summary>
    /// <param name="loggerFactory">The logger factory from the DI</param>
    public PdfViewerViewModel(ILoggerFactory loggerFactory)
    {
        logger = loggerFactory.CreateLogger<PdfViewerViewModel>();
    }
}
