using System.Threading;

using Voracious.Reader.Interface;

namespace Voracious.Reader.View;

public partial class PDFViewerPage : ContentPage
{
    private IPdfViewer context;
    
    public PDFViewerPage(IPdfViewer vm)
    {
        InitializeComponent();

        context = vm;
        BindingContext = vm;
    }

    /// <summary>
    /// It is used to delay the current thread's execution, 
    /// until the user enters the password.
    /// </summary>
    private ManualResetEvent manualResetEvent = new(false);

    /// <summary>
    /// Handles password requested event.
    /// </summary>
    private void PdfViewer_PasswordRequested(object sender, Syncfusion.Maui.PdfViewer.PasswordRequestedEventArgs e)
    {
        //Show the password dialog.
        PasswordDialog.Dispatcher.Dispatch(() => PasswordDialog.IsVisible = true);

        //Block the current thread until user enters the password.
        manualResetEvent.WaitOne();
        manualResetEvent.Reset();

        //Pass the user password to PDF Viewer to validate and load the PDF.
        e.Password = PasswordDialog.Password;
        e.Handled = true;
    }

    /// <summary>
    /// Handles when document is failed to load.
    /// </summary>
    private void PdfViewer_DocumentLoadFailed(object sender, Syncfusion.Maui.PdfViewer.DocumentLoadFailedEventArgs e)
    {
        PdfViewer.DocumentSource = null;
        //Show the incorrect password message to the user, when document failed to load if the password is invalid.
        if (e.Message == "Can't open an encrypted document. The password is invalid.")
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                MessageBox.Show("Incorrect Password", "The password you entered is incorrect. Please try again.");
            });
        }
    }

    /// <summary>
    /// Handles when the password is entered.
    /// </summary>
    private void PasswordDialogBox_PasswordEntered(object sender, EventArgs e)
    {
        //Hide the password dialog.
        PasswordDialog.IsVisible = false;

        //Release the current waiting thread to execute.
        manualResetEvent.Set();
    }

    /// <summary>
    /// Handles when the message box's ok button is clicked.
    /// </summary>
    private void MessageBox_OKClicked(object sender, EventArgs e)
    {
        if (BindingContext is IPdfViewer)
        {
            // Retry loading the document.
            PdfViewer.DocumentSource = context.PdfDocumentStream;
        }
    }

    // Event handler for Button click event, toggles the visibility of flattenOptions.
    private void Save_Clicked(object sender, EventArgs e)
    {
        // Toggles the visibility of flattenOptions.
        if (flattenOptions.IsVisible)
            flattenOptions.IsVisible = false; // Hides flattenOptions if it's currently visible.
        else
            flattenOptions.IsVisible = true; // Shows flattenOptions if it's currently hidden.
    }
}
