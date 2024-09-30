using Voracious.Reader.Interface;

namespace Voracious.Reader.View;

public partial class MainPage : ContentPage
{
    public MainPage(IMainPage vm)
    {
        InitializeComponent();

        BindingContext = vm;
    }
}

