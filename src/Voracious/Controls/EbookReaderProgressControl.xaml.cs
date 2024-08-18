// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

using Voracious.Interface;

namespace Voracious.Controls;

public sealed partial class EbookReaderProgressControl : ContentView, IProgressReader
{
    public EbookReaderProgressControl()
    {
        this.InitializeComponent();
    }

    public void AddLog(string log)
    {
        uiLog.Text += log;
    }

    public void SetCurrentBook(string title)
    {
        uiProgress.Value++;
        uiCurrentName.Text = title;
        uiLog.Text += $"Start: {title}\n";
    }

    public void SetNBooks(int nbooks)
    {
        uiProgress.Minimum = 0;
        uiProgress.Maximum = nbooks;
        uiProgress.Value = 0;
        uiLog.Text = "";
    }
}
