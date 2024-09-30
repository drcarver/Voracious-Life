namespace Voracious.Reader.Controls;

public class HyperlinkSpan : Span
{
    /// <summary>
    /// The HyperLink url
    /// </summary>
    public static readonly BindableProperty UrlProperty =
        BindableProperty.Create(nameof(Url), typeof(string), typeof(HyperlinkSpan), null);

    /// <summary>
    /// The Url backing property
    /// </summary>
    public string Url
    {
        get { return (string)GetValue(UrlProperty); }
        set { SetValue(UrlProperty, value); }
    }

    /// <summary>
    /// The for the tap recognizer
    /// </summary>
    public HyperlinkSpan()
    {
        TextDecorations = TextDecorations.Underline;
        TextColor = Colors.Blue;
        GestureRecognizers.Add(new TapGestureRecognizer
        {
            // Launcher.OpenAsync is provided by Essentials.
            Command = new Command(async () => await Launcher.OpenAsync(Url))
        });
    }
}