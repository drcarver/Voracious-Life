namespace Voracious.Control.PDF;

public partial class MessageBox : ContentView
{
    /// <summary>
    /// Constructor of the MessageBox
    /// </summary>
    public MessageBox()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Handles when Ok is clicked.
    /// </summary>
    private void Ok_Clicked(object sender, EventArgs e)
    {
        //Hide the message box.
        this.IsVisible = false;
    }

    /// <summary>
    /// Show the message box with the given title and message.
    /// </summary>
    public void Show(string title, string message, string closeText = "OK")
    {
        this.IsVisible = true;
        Title.Text = title;
        Message.Text = message;
        OkButton.Text = closeText;
    }
}