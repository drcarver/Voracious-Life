using Voracious.Reader.Interface;

namespace Voracious.Reader.View;

public partial class PDFViewerPage : ContentPage
{
	public PDFViewerPage(IPdfViewer vm)
	{
		InitializeComponent();
		
		BindingContext = vm;
	}

    private void PdfViewer_PasswordRequested(object sender, Syncfusion.Maui.PdfViewer.PasswordRequestedEventArgs e)
    {

    }

    private void PdfViewer_DocumentLoadFailed(object sender, Syncfusion.Maui.PdfViewer.DocumentLoadFailedEventArgs e)
    {

    }

    private void PasswordDialog_PasswordEntered(object sender, EventArgs e)
    {

    }

    private void MessageBox_OkClicked(object sender, EventArgs e)
    {

    }
}