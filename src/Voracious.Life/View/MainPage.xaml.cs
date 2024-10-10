using Voracious.Control.Interface;

namespace Voracious.Life.View;

public partial class MainPage : ContentPage
{
    public MainPage(IMainPage vm)
    {
        InitializeComponent();

        BindingContext = vm;
    }
}

