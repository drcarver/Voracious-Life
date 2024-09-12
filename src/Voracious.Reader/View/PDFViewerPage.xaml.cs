using Voracious.Reader.Interface;

namespace Voracious.Reader.View;

public partial class PDFViewerPage : ContentPage
{
	public PDFViewerPage(IPdfViewer vm)
	{
		InitializeComponent();
		
		BindingContext = vm;
	}
}